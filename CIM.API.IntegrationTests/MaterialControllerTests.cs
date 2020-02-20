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

namespace CIM.API.IntegrationTests
{
    public class MaterialControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var model = new MaterialModel()
            {
                Code = "A0001",
                Description = "Test description",
                ProductCategory = "Ingredient",
                ICSGroup = "Ingredient",
                MaterialGroup = "WHITE GRAPE JUICE",
                UOM = "KG",
                BHTPerUnit = 999,
                IsActive = true,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "api",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "api"
            };
            var content = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Act
            var createResponse = await TestClient.PostAsync("/api/Material/Create", byteContent);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponse = await TestClient.GetAsync("/api/Material/List?page=1&howmany=10");
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await listResponse.Content.ReadAsStringAsync()));

            // Assert            
            listResponseModel.HowMany.Should().Be(1);
            var id = listResponseModel.Data[0].Id;
            foreach (var m in listResponseModel.Data)
            {
                m.Code.Should().Be(model.Code);
                m.Description.Should().Be(model.Description);
                m.ProductCategory.Should().Be(model.ProductCategory);
                m.ICSGroup.Should().Be(model.ICSGroup);
                m.MaterialGroup.Should().Be(model.MaterialGroup);
                m.UOM.Should().Be(model.UOM);
                m.BHTPerUnit.Should().Be(model.BHTPerUnit);
                m.IsActive.Should().Be(model.IsActive);
                m.IsDelete.Should().Be(model.IsDelete);
                m.CreatedAt.Should().Be(model.CreatedAt);
                m.CreatedBy.Should().Be(model.CreatedBy);
                m.UpdatedAt.Should().Be(model.UpdatedAt);
                m.UpdatedBy.Should().Be(model.UpdatedBy);
            }
        }

        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var model = new MaterialModel()
            {
                Code = "A0001",
                Description = "Test description",
                ProductCategory = "Ingredient",
                ICSGroup = "Ingredient",
                MaterialGroup = "WHITE GRAPE JUICE",
                UOM = "KG",
                BHTPerUnit = 999,
                IsActive = true,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "api",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "api"
            };
            var content = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var createResponse = await TestClient.PostAsync("/api/Material/Create", byteContent);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponse = await TestClient.GetAsync("/api/Material/List?page=1&howmany=10");
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await listResponse.Content.ReadAsStringAsync()));

            listResponseModel.HowMany.Should().Be(1);
            var id = listResponseModel.Data[0].Id;
            foreach (var m in listResponseModel.Data)
            {
                m.Code.Should().Be(model.Code);
                m.Description.Should().Be(model.Description);
                m.ProductCategory.Should().Be(model.ProductCategory);
                m.ICSGroup.Should().Be(model.ICSGroup);
                m.MaterialGroup.Should().Be(model.MaterialGroup);
                m.UOM.Should().Be(model.UOM);
                m.BHTPerUnit.Should().Be(model.BHTPerUnit);
                m.IsActive.Should().Be(model.IsActive);
                m.IsDelete.Should().Be(model.IsDelete);
                m.CreatedAt.Should().Be(model.CreatedAt);
                m.CreatedBy.Should().Be(model.CreatedBy);
                m.UpdatedAt.Should().Be(model.UpdatedAt);
                m.UpdatedBy.Should().Be(model.UpdatedBy);
            }
            // Act
            var response = await TestClient.GetAsync("/api/Material/Get?id=" + id);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseModel = JsonConvert.DeserializeObject<MaterialModel>((await response.Content.ReadAsStringAsync()));

            // Assert            
            responseModel.Code.Should().Be(model.Code);
            responseModel.Description.Should().Be(model.Description);
            responseModel.ProductCategory.Should().Be(model.ProductCategory);
            responseModel.ICSGroup.Should().Be(model.ICSGroup);
            responseModel.MaterialGroup.Should().Be(model.MaterialGroup);
            responseModel.UOM.Should().Be(model.UOM);
            responseModel.BHTPerUnit.Should().Be(model.BHTPerUnit);
            responseModel.IsActive.Should().Be(model.IsActive);
            responseModel.IsDelete.Should().Be(model.IsDelete);
            responseModel.CreatedAt.Should().Be(model.CreatedAt);
            responseModel.CreatedBy.Should().Be(model.CreatedBy);
            responseModel.UpdatedAt.Should().Be(model.UpdatedAt);
            responseModel.UpdatedBy.Should().Be(model.UpdatedBy);
        }

        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var model = new MaterialModel()
            {
                Code = "A0001",
                Description = "Test description",
                ProductCategory = "Ingredient",
                ICSGroup = "Ingredient",
                MaterialGroup = "WHITE GRAPE JUICE",
                UOM = "KG",
                BHTPerUnit = 999,
                IsActive = true,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                CreatedBy = "api",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "api"
            };
            var content = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var createResponse = await TestClient.PostAsync("/api/Material/Create", byteContent);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponse = await TestClient.GetAsync("/api/Material/List?page=1&howmany=10");
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var listResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await listResponse.Content.ReadAsStringAsync()));

            listResponseModel.HowMany.Should().Be(1);
            var id = listResponseModel.Data[0].Id;
            foreach (var m in listResponseModel.Data)
            {
                m.Code.Should().Be(model.Code);
                m.Description.Should().Be(model.Description);
                m.ProductCategory.Should().Be(model.ProductCategory);
                m.ICSGroup.Should().Be(model.ICSGroup);
                m.MaterialGroup.Should().Be(model.MaterialGroup);
                m.UOM.Should().Be(model.UOM);
                m.BHTPerUnit.Should().Be(model.BHTPerUnit);
                m.IsActive.Should().Be(model.IsActive);
                m.IsDelete.Should().Be(model.IsDelete);
                m.CreatedAt.Should().Be(model.CreatedAt);
                m.CreatedBy.Should().Be(model.CreatedBy);
                m.UpdatedAt.Should().Be(model.UpdatedAt);
                m.UpdatedBy.Should().Be(model.UpdatedBy);
            }



            var updateModel = new MaterialModel()
            {
                Id = id,
                Code = "A0001",
                Description = "Test description 2",
                ProductCategory = "Ingredient",
                ICSGroup = "Ingredient",
                MaterialGroup = "WHITE GRAPE JUICE",
                UOM = "KG",
                BHTPerUnit = 999,
                IsActive = true,
                IsDelete = false,
                //CreatedAt = DateTime.Now,
                CreatedBy = "api",
                //UpdatedAt = DateTime.Now,
                UpdatedBy = "api"
            };
            var updateContent = JsonConvert.SerializeObject(updateModel);
            var updateBuffer = System.Text.Encoding.UTF8.GetBytes(updateContent);
            var updateByteContent = new ByteArrayContent(updateBuffer);
            updateByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Act
            var updateResponse = await TestClient.PostAsync("/api/Material/Update", updateByteContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            // Assert
            var checkListResponse = await TestClient.GetAsync("/api/Material/List?page=1&howmany=10");
            listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var checkListResponseModel = JsonConvert.DeserializeObject<PagingModel<MaterialModel>>((await checkListResponse.Content.ReadAsStringAsync()));

            checkListResponseModel.HowMany.Should().Be(1);
            foreach (var m in listResponseModel.Data)
            {
                m.Code.Should().Be(updateModel.Code);
                m.Description.Should().Be(updateModel.Description);
                m.ProductCategory.Should().Be(updateModel.ProductCategory);
                m.ICSGroup.Should().Be(updateModel.ICSGroup);
                m.MaterialGroup.Should().Be(updateModel.MaterialGroup);
                m.UOM.Should().Be(updateModel.UOM);
                m.BHTPerUnit.Should().Be(updateModel.BHTPerUnit);
                m.IsActive.Should().Be(updateModel.IsActive);
                m.IsDelete.Should().Be(updateModel.IsDelete);
                m.CreatedAt.Should().Be(updateModel.CreatedAt);
                m.CreatedBy.Should().Be(updateModel.CreatedBy);
                m.UpdatedAt.Should().Be(updateModel.UpdatedAt);
                m.UpdatedBy.Should().Be(updateModel.UpdatedBy);
            }

        }
    }
}
