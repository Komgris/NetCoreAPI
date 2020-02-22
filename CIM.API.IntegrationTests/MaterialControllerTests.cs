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
    public class MaterialControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var code = "TestCreate001";
            var model = await CreateDataTest(code);
            var expectedCount = 1;

            // Act
            var listResponse = (await TestClient.GetAsync("/api/Material/List?page=1&howmany=10"));
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count(x=>x.Code== code).Should().Be(expectedCount);
            var responseModel = listResponseModel.Data.First(x => x.Code == code);

           TestHelper.CompareModel(responseModel, model);            
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var code = "TestGet001";
            var model = await CreateDataTest(code);

            // Act
            var response = await TestClient.GetAsync("/api/Material/Get?id=" + model.Id);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MaterialModel>((await response.Content.ReadAsStringAsync()));

            // Assert            
            TestHelper.CompareModel(responseModel, model);
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var code = "TestUpdate001";
            var model = await CreateDataTest(code);

            model.Description = "Update Description";


            var updateContent = JsonConvert.SerializeObject(model);
            var updateBuffer = System.Text.Encoding.UTF8.GetBytes(updateContent);
            var updateByteContent = new ByteArrayContent(updateBuffer);
            updateByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Act
            var updateResponse = await TestClient.PostAsync("/api/Material/Update", updateByteContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseModel = JsonConvert.DeserializeObject<MaterialModel>((await updateResponse.Content.ReadAsStringAsync()));

            var response = await TestClient.GetAsync("/api/Material/Get?id=" + model.Id);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MaterialModel>((await response.Content.ReadAsStringAsync()));

            // Assert
            TestHelper.CompareModel(responseModel, updateResponseModel);
        }
        public async Task<MaterialModel> CreateDataTest(string code)
        {
            var model = TestHelper.GetMock(code);

            var content = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await TestClient.PostAsync("/api/Material/Create", byteContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MaterialModel>((await response.Content.ReadAsStringAsync()));

            return responseModel;// responseModel.Id;
        }
    }
}
