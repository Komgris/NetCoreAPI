using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CIM.API.IntegrationTests
{
    public class UserControllerTest : IntegrationTest
    {
        [Fact]
        public async Task Register_Test()
        {
            // Arrange

            // Act
            var registerUserModel = new RegisterUserModel
            {
                Email = "test@email.com",
                UserName = "user1",
                Password = "super-secret"
            };
            var content = new StringContent(JsonConvert.SerializeObject(registerUserModel));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await TestClient.PostAsync("User", content);
            var authResp = await TestClient.GetAsync($"User?username={registerUserModel.UserName}&password={registerUserModel.Password}");
            var authResult = JsonConvert.DeserializeObject<AuthModel>((await authResp.Content.ReadAsStringAsync()));
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            authResp.StatusCode.Should().Be(HttpStatusCode.OK);
            authResult.UserId.Should().NotBeNullOrEmpty();
            authResult.Name.Should().NotBeNullOrEmpty();

        }

    }
}
