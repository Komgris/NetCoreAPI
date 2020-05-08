using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using System.Data;
using CIM.Model;

namespace CIM.API.IntegrationTests {
    public class ReportControllerTests : IntegrationTest {
        [Fact]
        public async Task GetProductionSummary_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionSummary?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetMachineSpeed_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetMachineSpeed?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetProductionPlanInfomation_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionPlanInfomation?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetProductionOperators_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionOperators?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetProductionEvents_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionEvents?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetProductionLossHistory_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionLossHistory?planid=1&routeid=1&page=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetCapacityUtilisation_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetCapacityUtilisation?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetWasteByMaterials_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetWasteByMaterials?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }
        
        [Fact]
        public async Task GetWasteByCases_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetWasteByCases?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }
        
        [Fact]
        public async Task GetWasteByMachines_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetWasteByMachines?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }
        
        [Fact]
        public async Task GetWasteCostByTime_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetWasteCostByTime?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }
        
        [Fact]
        public async Task GetWasteHistory_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetWasteHistory?planid=1&routeid=1&page=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetActiveMachineInfo_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetActiveMachineInfo?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetActiveMachineEvents_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetActiveMachineEvents?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetMachineStatusHistory_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetMachineStatusHistory?planid=1&routeid=1&page=1&howMany=15");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }
    }
}
