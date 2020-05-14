using CIM.Model;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using System.Linq;
using Xunit;
using Newtonsoft.Json;
using CIM.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Collections.Generic;

namespace CIM.API.IntegrationTests
{
    public class ComponentControllerTest : BaseIntegrationTest
    {
        TestScenario scenario;
        public ComponentControllerTest()
        {
            scenario = CreateWebApplication();
        }

        public Component Get(string name, TestScenario scenario)
        {
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                return context.Component.First(x => x.Name == name);
            }
        }

        [Fact]
        public async Task Create_test()
        {
            var component = new ComponentModel
            {
                Name = "Component1",
                IsActive = true
            };

            var content = GetHttpContentForPost(component, scenario.AdminToken);
            var createResponse = await scenario.TestClient.PostAsync("/api/Component/Create", content);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = Get(component.Name, scenario);
            result.Name.Should().Be(component.Name);
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var component = new Component
            {
                Name = "Component2",
                IsActive = true
            };

            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.Component.Add(component);
                context.SaveChanges();
            }

            var resultId = Get(component.Name, scenario);


            var loadResponse = await scenario.TestClient.GetAsync($"api/Component/Get?id={resultId.Id}");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loadResponseString = await loadResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<ComponentModel>>(loadResponseString);

            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be(component.Name);
        }
        [Fact]
        public async Task Insert_Mapping()
        {
            var componentList = new List<Component>()
            {
                new Component{ Name="testA",MachineId=3},
                new Component{ Name="testB",MachineId=3},
                new Component{ Name="testC",MachineId=3},
                new Component{ Name="testD",MachineId=5},
            };
            foreach (var model in componentList)
            {
                using (var scope = scenario.ServiceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<cim_dbContext>();
                    context.Component.Add(model);
                    context.SaveChanges();
                }
            }
            var mapping = new MappingMachineComponent()
            {
                MachineId = 3,
                ComponentList = new List<ComponentModel>()
                {
                    new ComponentModel{ Name="testA"},
                    new ComponentModel{ Name="testC"},
                    new ComponentModel{ Name="testD"},
                }
            };
            foreach(var model in mapping.ComponentList)
            {
                model.Id = Get(model.Name, scenario).Id;
            }

            var content = GetHttpContentForPost(mapping, scenario.AdminToken);
            var updateResponse = await scenario.TestClient.PostAsync("/api/Component/InsertMappingMachineComponent", content);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            foreach(var model in mapping.ComponentList)
            {
                var result = Get(model.Name, scenario);
                result.MachineId.Should().Be(mapping.MachineId);
            }
            var testNull = Get("testB", scenario).MachineId;
            testNull.Should().BeNull();
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var component = new Component
            {
                Name = "Component3",
                IsActive = true
            };

            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.Component.Add(component);
                context.SaveChanges();
            }
            var getId = Get(component.Name, scenario);

            var componentEdit = new ComponentModel
            {
                Id = getId.Id,
                Name = "ComponentEdit",
                IsActive = true
            };

            var content = GetHttpContentForPost(componentEdit, scenario.AdminToken);
            var updateResponse = await scenario.TestClient.PutAsync("/api/Component/Update", content);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = Get(componentEdit.Name, scenario);
            result.Name.Should().Be(componentEdit.Name);
        }

        [Fact]
        public async Task Get_No_MachineId()
        {
            var componentList = new List<Component>()
            {
                new Component{ Name="testA",MachineId=3},
                new Component{ Name="testB",MachineId=3},
                new Component{ Name="testC",MachineId=null},
                new Component{ Name="testD",MachineId=null},
            };

            foreach (var model in componentList)
            {
                using (var scope = scenario.ServiceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<cim_dbContext>();
                    context.Component.Add(model);
                    context.SaveChanges();
                }
            }

            var loadResponse = await scenario.TestClient.GetAsync($"api/Component/GetComponentNoMachineId");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loadResponseString = await loadResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<List<ComponentModel>>>(loadResponseString);

            result.Data.Should().NotBeNull();
            result.Data.Count.Should().Be(2);
        }
    }
}
