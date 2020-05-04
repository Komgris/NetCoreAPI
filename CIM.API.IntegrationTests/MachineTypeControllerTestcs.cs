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
    public class MachineTypeControllerTestcs : BaseIntegrationTest
    {
        TestScenario scenario;

        public MachineTypeControllerTestcs()
        {
            //Run on each test
            scenario = CreateWebApplication();
            //Setup(scenario);
        }

        public MachineType Get(string name, TestScenario scenario)
        {
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                return context.MachineType.First(x => x.Name == name);
            }
        }

        [Fact]
        public async Task Create_test()
        {
            var machineType = new MachineTypeModel
            {
                Id = 1,
                Name = "Machine1"
            };

            var content = GetHttpContentForPost(machineType, scenario.AdminToken);
            var createResponse = await scenario.TestClient.PostAsync("/api/MachineType/Create", content);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = Get(machineType.Name, scenario);
            result.Name.Should().Be(machineType.Name);
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var machineType = new MachineType
            {
                Id= 2,
                Name = "Machine_test",
                IsActive = true
            };

            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.MachineType.Add(machineType);
                context.SaveChanges();
            }

            var resultId = Get(machineType.Name, scenario);


            var loadResponse = await scenario.TestClient.GetAsync($"api/MachineType/Get?id={resultId.Id}");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loadResponseString = await loadResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<MachineTypeModel>>(loadResponseString);

            result.Data.Should().NotBeNull();
            result.Data.Name.Should().Be(machineType.Name);
        }


        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var machineType = new MachineType
            {
                Id = 3,
                Name = "Machine",
                IsActive = true
            };

            var machineTypeEdit = new MachineType
            {
                Id = 3,
                Name = "Machine_edit",
                IsActive = true
            };

            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.MachineType.Add(machineType);
                context.SaveChanges();
            }

            var content = GetHttpContentForPost(machineTypeEdit, scenario.AdminToken);
            var updateResponse = await scenario.TestClient.PutAsync("/api/MachineType/Update", content);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = Get(machineTypeEdit.Name, scenario);
            result.Name.Should().Be(machineTypeEdit.Name);
        }



    }
}
