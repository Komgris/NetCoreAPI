﻿using CIM.Model;
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
        #region Test function
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var code = "TestCreate001";
            var model = await CreateData(code);
            var expectedCount = 1;

            // Act
            var listResponse = (await TestClient.GetAsync("/api/Material/List?page=1&howmany=10"));

            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count(x => x.Code == code).Should().Be(expectedCount);
            var responseModel = listResponseModel.Data.First(x => x.Code == code);

            MaterialTestHelper.CompareModel(responseModel, model);
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var code = "TestGet001";
            var model = await CreateData(code);

            // Act
            var response = await TestClient.GetAsync("/api/Material/Get?id=" + model.Id);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MaterialModel>((await response.Content.ReadAsStringAsync()));

            // Assert            
            MaterialTestHelper.CompareModel(responseModel, model);
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var code = "TestUpdate001";
            var model = await CreateData(code);
            var token = string.Empty;

            model.Description = "Update Description";

            var updateByteContent = GetHttpContentForPost(model, token);

            // Act
            var updateResponse = await TestClient.PostAsync("/api/Material/Update", updateByteContent);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseModel = JsonConvert.DeserializeObject<MaterialModel>((await updateResponse.Content.ReadAsStringAsync()));


            var response = await TestClient.GetAsync("/api/Material/Get?id=" + model.Id);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MaterialModel>((await response.Content.ReadAsStringAsync()));

            // Assert
            MaterialTestHelper.CompareModel(responseModel, updateResponseModel);
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
            var listResponse = (await TestClient.GetAsync("/api/Material/List?page=1&howmany=" + expectedCount));
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await listResponse.Content.ReadAsStringAsync()));

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
            var listResponse = (await TestClient.GetAsync(string.Format("/api/Material/List?keyword={0}&page=1&howmany=2", expectedKeyword)));
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count().Should().Be(expectedKCount);
            foreach (var model in listResponseModel.Data)
                model.Code.Should().Contain(expectedKeyword);
        }
        #endregion

        #region Create data for use in test function
        public async Task<MaterialModel> CreateData(string code)
        {
            var model = MaterialTestHelper.GetMock(code);
            var token = string.Empty;

            var content = GetHttpContentForPost(model, token);
            var response = await TestClient.PostAsync("/api/Material/Create", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MaterialModel>((await response.Content.ReadAsStringAsync()));

            return responseModel;
        }
        #endregion

    }
}
