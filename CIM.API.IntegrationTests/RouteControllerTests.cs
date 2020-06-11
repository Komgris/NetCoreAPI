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
    public class RouteControllerTests : BaseIntegrationTest
    {
        TestScenario scenario;

        public RouteControllerTests()
        {
            scenario = CreateWebApplication();
        }

        public Route Get(string name, TestScenario scenario)
        {
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                return context.Route.First(x => x.Name == name);
            }
        }

        [Fact]
        public async Task Create_test()
        {
            var route = new RouteModel
            {
                Name = "Route1"
            };

            var content = GetHttpContentForPost(route, scenario.AdminToken);
            var createResponse = await scenario.TestClient.PostAsync("/api/Route/Create", content);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = Get(route.Name, scenario);
            result.Name.Should().Be(route.Name);
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var route = new Route
            {
                Name = "Route2",
                IsActive = true
            };

            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.Route.Add(route);
                context.SaveChanges();
            }

            var resultId = Get(route.Name, scenario);


            var loadResponse = await scenario.TestClient.GetAsync($"api/Route/Get?id={resultId.Id}");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loadResponseString = await loadResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<RouteListModel>>(loadResponseString);

            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be(route.Name);
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var route = new Route
            {
                Name = "Route3",
                IsActive = true
            };

            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.Route.Add(route);
                context.SaveChanges();
            }

            var getId = Get(route.Name, scenario);

            var routeEdit = new ComponentTypeModel
            {
                Id = getId.Id,
                Name = "RouteEdit",
                IsActive = true
            };

            var content = GetHttpContentForPost(routeEdit, scenario.AdminToken);
            var updateResponse = await scenario.TestClient.PutAsync("/api/Route/Update", content);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = Get(routeEdit.Name, scenario);
            result.Name.Should().Be(routeEdit.Name);
        }
    }
}
