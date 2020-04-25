using System;
using System.Collections.Generic;
using System.Text;

using CIM.Model;
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
    public class LossControllerTest : IntegrationTest
    {
        [Fact]
        public async Task GetLossLevel1List_Test()
        {
            //https://localhost:44365/api/LossLevel1/List?page=1&howmany=10&isActive=true
            // Act
            var response = await TestClient.GetAsync("/api/LossLevel1/List?page=1&howmany=10&isActive=true");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetLossLevel2List_Test()
        {
            //https://localhost:44365/api/LossLevel1/List?page=1&howmany=10&isActive=true
            // Act
            var response = await TestClient.GetAsync("/api/LossLevel2/List?page=1&howmany=10&isActive=true");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetLossLevel3List_Test()
        {
            //https://localhost:44365/api/LossLevel1/List?page=1&howmany=10&isActive=true
            // Act
            var response = await TestClient.GetAsync("/api/LossLevel3/List?page=1&howmany=10&isActive=true");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

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