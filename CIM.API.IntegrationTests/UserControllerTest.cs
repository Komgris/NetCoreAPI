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
using CIM.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CIM.API.IntegrationTests
{
    public class UserControllerTest : IntegrationTest
    {
        [Fact]
        public async Task RegisterAndAuth_Test()
        {
            // Arrange
            var registerUserModel = new RegisterUserModel
            {
                Email = "test@email.com",
                UserName = "user1",
                Password = "super-secret",
                FirstName = "Hans",
                LastName = "Meier",
                Image = null,

            };

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                var testGroup = new UserGroups
                {
                    IsActive = true,
                    UserGroupLocal = new List<UserGroupLocal> {
                                new  UserGroupLocal { LanguageId = "en", Name = "TestGroup", }
                            }
                };
                context.UserGroups.Add(testGroup);
                context.SaveChanges();
                registerUserModel.UserGroup_Id = testGroup.Id;
            }

            // Act
            var content = GetHttpContentForPost(registerUserModel);
            var response = await TestClient.PostAsync("User", content);
            var authResp = await TestClient.GetAsync($"User?username={registerUserModel.UserName}&password={registerUserModel.Password}");
            var authResult = JsonConvert.DeserializeObject<AuthModel>((await authResp.Content.ReadAsStringAsync()));
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            authResp.StatusCode.Should().Be(HttpStatusCode.OK);
            authResult.UserId.Should().NotBeNullOrEmpty();
            authResult.IsSuccess.Should().BeTrue();
            authResult.FullName.Should().Be(registerUserModel.FirstName + " " + registerUserModel.LastName);

        }

        [Fact]
        public async Task Auth_WhenLoginWithWrongUser_Test()
        {
            var authResp = await TestClient.GetAsync($"User?username=unknow&password=unknow");
            var authResult = JsonConvert.DeserializeObject<AuthModel>((await authResp.Content.ReadAsStringAsync()));
            authResult.UserId.Should().BeNull();
            authResult.IsSuccess.Should().BeFalse();
            authResult.FullName.Should().BeNull();
        }

    }
}
