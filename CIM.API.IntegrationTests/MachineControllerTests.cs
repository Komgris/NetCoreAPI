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
using CIM.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using CIM.BusinessLogic.Utility;

namespace CIM.API.IntegrationTests
{
    public class MachineControllerTests : IntegrationTest
    {
        #region Test function

        public Machine GetMachineId(string name)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                return context.Machine.First(x => x.Name == name);
            }
        }


        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var name = "Machine01";
            var model = new MachineListModel()
            {
                Name = name,
                StatusId = 1,
                MachineTypeId = 2,
                Type = "Sealing",
                IsActive = true,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "TEST",
            };

            var content = GetHttpContentForPost(model, AdminToken);
            var response = await TestClient.PostAsync("/api/Machine/Create", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MachineListModel>((await response.Content.ReadAsStringAsync()));

            // Act
            var dbModel = GetMachineId(name);
            var expected = MapperHelper.AsModel(dbModel, new MachineListModel());
            model.Name.Should().Be(expected.Name);
            model.StatusId.Should().Be(expected.StatusId);
            model.MachineTypeId.Should().Be(expected.MachineTypeId);
            model.IsActive.Should().Be(expected.IsActive);
            model.IsDelete.Should().Be(expected.IsDelete);
        }

        [Fact(Skip = "Need to Fix later")]
        public async Task Get_Test()
        {
            // Arrange
            var model = new Machine
            {
                Name = "SOME_MACHINE1"
            };

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.Machine.Add(model);
                context.SaveChanges();
            }

            // Act
            var response = await TestClient.GetAsync("/api/Machine/Get?id=" + model.Id);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<ProcessReponseModel<MachineListModel>>((await response.Content.ReadAsStringAsync()));

            // Assert            
            var expected = MapperHelper.AsModel(model, new MachineListModel());
            responseModel.Data.Name.Should().Be(expected.Name);
            responseModel.Data.StatusId.Should().Be(expected.StatusId);
            responseModel.Data.MachineTypeId.Should().Be(expected.MachineTypeId);
            responseModel.Data.IsActive.Should().Be(expected.IsActive);
            responseModel.Data.IsDelete.Should().Be(expected.IsDelete);
        }

        [Fact(Skip = "Need to Fix later")]
        public async Task Update_Test()
        {
            // Arrange
            var code = "TestUpdate001";
            var model = await CreateData(code);
            var token = string.Empty;


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

        [Fact(Skip = "Need to Fix later")]
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

        [Fact(Skip = "Need to Fix later")]
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
