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
        public async Task List_Test()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/Material/List");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Test()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/Material/Get/10");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Insert_Test()
        {
            // Arrange
            var model = new MaterialModel()
            {
                Code = "A0001",
                Description = "Test description",
                CreatedBy = "api",
                UpdatedBy = "api"
            };
            var content = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Act
            var response = await TestClient.PostAsync("/api/Material/Insert", byteContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            var model = new MaterialModel()
            {
                Code = "A0001",
                Description = "Test description2",
                CreatedBy = "api",
                UpdatedBy = "api"
            };
            var content = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Act
            Insert_Test();
            var response = await TestClient.PostAsync("/api/Material/Update", byteContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
