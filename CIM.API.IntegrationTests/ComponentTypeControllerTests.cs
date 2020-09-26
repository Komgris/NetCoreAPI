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
    public class ComponentTypeControllerTestcs : BaseIntegrationTest
    {
        TestScenario scenario;

        public ComponentTypeControllerTestcs()
        {
            scenario = CreateWebApplication();
        }

        public ComponentType Get(string name, TestScenario scenario)
        {
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                return context.ComponentType.First(x => x.Name == name);
            }
        }

        [Fact]
        public async Task Create_test()
        {
            var componentType = new ComponentTypeModel
            {
                Name = "Component1"
            };

            var content = GetHttpContentForPost(componentType, scenario.AdminToken);
            var createResponse = await scenario.TestClient.PostAsync("/api/ComponentType/Create", content);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = Get(componentType.Name, scenario);
            result.Name.Should().Be(componentType.Name);
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var componentType = new ComponentType
            {
                Name = "Component2",
                IsActive = true
            };

            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ComponentType.Add(componentType);
                context.SaveChanges();
            }

            var resultId = Get(componentType.Name, scenario);


            var loadResponse = await scenario.TestClient.GetAsync($"api/ComponentType/Get?id={resultId.Id}");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loadResponseString = await loadResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<ComponentTypeModel>>(loadResponseString);

            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be(componentType.Name);
        }


        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var componentType = new ComponentType
            {
                Name = "Component3",
                IsActive = true
            };

            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ComponentType.Add(componentType);
                context.SaveChanges();
            }
            var getId = Get(componentType.Name, scenario);

            var componentTypeEdit = new ComponentTypeModel
            {
                Id = getId.Id,
                Name = "ComponentEdit",
                IsActive = true
            };

            var content = GetHttpContentForPost(componentTypeEdit, scenario.AdminToken);
            var updateResponse = await scenario.TestClient.PutAsync("/api/ComponentType/Update", content);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = Get(componentTypeEdit.Name, scenario);
            result.Name.Should().Be(componentTypeEdit.Name);
        }



    }
}
