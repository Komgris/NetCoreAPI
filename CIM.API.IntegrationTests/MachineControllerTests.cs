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
    public class MachineControllerTests : IntegrationTest
    {
        #region Test function
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var name = "Machine01";
            var model = await CreateData(name);
            var expectedCount = 1;
            var page = 1;
            var pageSize = 10;

            // Act
            var listResponse = (await TestClient.GetAsync($"/api/Machine/{page}/{pageSize}"));

            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MachineListModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count(x => x.Name == name).Should().Be(expectedCount);
            var responseModel = listResponseModel.Data.First(x => x.Name == name);

            MachineTestHelper.CompareModel(responseModel, model);
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var name = "Machine01";
            var model = await CreateData(name);

            // Act
            var response = await TestClient.GetAsync("/api/Machine/" + model.Id);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MachineListModel>((await response.Content.ReadAsStringAsync()));

            // Assert            
            MachineTestHelper.CompareModel(responseModel, model);
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var code = "TestUpdate001";
            var model = await CreateData(code);
            var token = string.Empty;

            model.Plcaddress = "Update PLC Address";

            var updateByteContent = GetHttpContentForPost(model, token);

            // Act
            var updateResponse = await TestClient.PostAsync("/api/Machine/Update", updateByteContent);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseModel = JsonConvert.DeserializeObject<MachineListModel>((await updateResponse.Content.ReadAsStringAsync()));


            var response = await TestClient.GetAsync("/api/Machine/Get?id=" + model.Id);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MachineListModel>((await response.Content.ReadAsStringAsync()));

            // Assert
            MachineTestHelper.CompareModel(responseModel, updateResponseModel);
        }

        [Fact]
        public async Task List_HowMany_Test()
        {
            // Arrange
            var model1 = await CreateData("TestList001");
            var model2 = await CreateData("TestList002");
            var model3 = await CreateData("TestList003");
            var expectedCount = 2;

            // Act
            var listResponse = (await TestClient.GetAsync("/api/Machine/List?page=1&howmany=" + expectedCount));
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MachineListModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count().Should().Be(expectedCount);
        }

        [Fact]
        public async Task List_Keyword_Test()
        {
            // Arrange
            var model1 = await CreateData("TestList001AA");
            var model2 = await CreateData("TestList002BB");
            var model3 = await CreateData("TestList003AA");
            var expectedKeyword = "AA";
            var expectedKCount = 2;

            // Act
            var listResponse = (await TestClient.GetAsync(string.Format("/api/Machine/List?keyword={0}&page=1&howmany=2", expectedKeyword)));
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MachineListModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count().Should().Be(expectedKCount);
            foreach (var model in listResponseModel.Data)
                model.Name.Should().Contain(expectedKeyword);
        }
        #endregion

        #region Create data for use in test function
        public async Task<MachineListModel> CreateData(string code)
        {
            var model = MachineTestHelper.GetMock(code);
            var token = string.Empty;

            var content = GetHttpContentForPost(model, token);
            var response = await TestClient.PostAsync("/api/Machine/Create", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MachineListModel>((await response.Content.ReadAsStringAsync()));

            return responseModel;
        }
        #endregion

    }
}
