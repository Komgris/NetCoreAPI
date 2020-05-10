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
    }
}
