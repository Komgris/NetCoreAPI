using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using System.Data;

namespace CIM.API.IntegrationTests {
    public class ReportControllerTests : IntegrationTest {
        [Fact]
        public async Task GetProductionSummary_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionSummary?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DataTable>(loadResponseString);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetMachineSpeed_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetMachineSpeed?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DataTable>(loadResponseString);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetProductionPlanInfomation_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionPlanInfomation?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DataTable>(loadResponseString);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetProductionOperators_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionOperators?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DataTable>(loadResponseString);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetProductionEvents_Test() {

            // Act
            var response = await TestClient.GetAsync("/api/Report/GetProductionEvents?planid=1&routeid=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DataTable>(loadResponseString);
            result.Should().NotBeNull();
        }
    }
}
