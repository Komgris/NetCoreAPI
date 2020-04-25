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
            // Act
            var response = await TestClient.GetAsync("/api/LossLevel3/List?page=1&howmany=10&isActive=true");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var loadResponseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<object>>(loadResponseString);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateLossLevel3_Test()
        {
            // Arrange
            var name = "Test";
            var id = 300;
            var model = await CreateData(id,name);
            var token = string.Empty;
            model.Description = "Update Description";
            var updateByteContent = GetHttpContentForPost(model, token);

            // Act
            var updateResponse = await TestClient.PostAsync("/api/LossLevel3/Update", updateByteContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseModel = JsonConvert.DeserializeObject<ProcessReponseModel<LossLevel3Model>>((await updateResponse.Content.ReadAsStringAsync()));

            var response = await TestClient.GetAsync("/api/LossLevel3/Get?id=" + model.Id);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var getresponseModel   = JsonConvert.DeserializeObject<ProcessReponseModel<LossLevel3Model>>((await response.Content.ReadAsStringAsync()));

            // Assert
            LossTestHelper.CompareLevel3Model(getresponseModel.Data, updateResponseModel.Data);
        }

        public async Task<LossLevel3Model> CreateData(int id, string name)
        {
            var model = LossTestHelper.GetMockLevel3(id,name);
            var token = string.Empty;
            var content = GetHttpContentForPost(model, token);
            var response = await TestClient.PostAsync("/api/LossLevel3/Create", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<LossLevel3Model>((await response.Content.ReadAsStringAsync()));
            return responseModel;
        }
    }
}