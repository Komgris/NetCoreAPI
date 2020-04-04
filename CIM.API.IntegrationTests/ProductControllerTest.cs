using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using CIM.DAL.Interfaces;
using Moq;
using CIM.BusinessLogic.Services;
using System.Linq;
using CIM.Domain.Models;
using CIM.API.IntegrationTests.Helper;

namespace CIM.API.IntegrationTests
{
    public class ProductControllerTest : IntegrationTest
    {
        #region Test function
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var name = "ProductA";
            var model = await CreateDataList(name);
            
            var expectedCount = 1;
            var page = 1;
            var pageSize = 10;

            // Act
            var listResponse = (await TestClient.GetAsync($"/api/Product/List/{page}/{pageSize}"));

            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<ProductModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count(x => x.Code == name).Should().Be(expectedCount);
            var responseModel = listResponseModel.Data.First(x => x.Code == name);

            ProductTestHelper.CompareModelProductList(model.Data, responseModel);
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var name = "ProductA";
            var model = await CreateData(name);

            // Act
            var response = await TestClient.GetAsync("/api/Product/" + model.Id);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<ProductModel>((await response.Content.ReadAsStringAsync()));

            // Assert            
            ProductTestHelper.CompareModelProduct(responseModel, model);
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var code = "updateProductA";
            var model = await CreateData(code);
            var token = string.Empty;


            var updateByteContent = GetHttpContentForPost(model, token);

            // Act
            var updateResponse = await TestClient.PostAsync("/api/Product/Update", updateByteContent);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseModel = JsonConvert.DeserializeObject<MachineListModel>((await updateResponse.Content.ReadAsStringAsync()));


            var response = await TestClient.GetAsync("/api/Product/Get?id=" + model.Id);

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
            var listResponse = (await TestClient.GetAsync("/api/Product/List?page=1&howmany=" + expectedCount));
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<ProductModel>>((await listResponse.Content.ReadAsStringAsync()));

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

            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<ProductModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert       
            listResponseModel.Data.Count().Should().Be(expectedKCount);
            foreach (var model in listResponseModel.Data)
                model.Code.Should().Contain(expectedKeyword);
        }
        #endregion

        #region Create data for use in test function
        public async Task<ProductModel> CreateData(string code)
        {
            var model = ProductTestHelper.GetProduct(code);
            var token = string.Empty;
            var productList = new List<ProductModel> { model };

            var content = GetHttpContentForPost(productList, AdminToken);
            var response = await TestClient.PostAsync("/api/Product/Create", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<ProductModel>((await response.Content.ReadAsStringAsync()));

            return responseModel;
        }
        public async Task<ProcessReponseModel<List<ProductModel>>> CreateDataList(string data)
        {
            var productList = ProductTestHelper.GetProductList();

            var content = GetHttpContentForPost(productList, AdminToken);
            var response = await TestClient.PostAsync("/api/Product/Create", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<ProcessReponseModel<List<ProductModel>>>((await response.Content.ReadAsStringAsync()));

            return responseModel;
        }

        #endregion
    }
}
