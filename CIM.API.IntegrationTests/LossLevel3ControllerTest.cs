using System;
using System.Collections.Generic;
using System.Text;

using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using CIM.API.IntegrationTests.Helper;
using System.Linq;

namespace CIM.API.IntegrationTests
{
    public class LossLevel3ControllerTest : IntegrationTest
    {
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var code = "TestCreate001";
            //var model = await CreateData(code);
            var expectedCount = 1;

            // Act
            var listResponse = (await TestClient.GetAsync("/api/Material/List?page=1&howmany=10"));

            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count(x => x.Code == code).Should().Be(expectedCount);
            var responseModel = listResponseModel.Data.First(x => x.Code == code);

           // MaterialTestHelper.CompareModel(responseModel, model);
        }
    }
}
