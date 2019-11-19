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
            var testGroup = new UserGroups
            {
                IsActive = true,
                UserGroupLocal = new List<UserGroupLocal> {
                                new  UserGroupLocal { LanguageId = "en", Name = "TestGroup", }
                            }
            };

            var app1 = new App { IsActive = true, Name = "App1" };
            var app2 = new App { IsActive = true, Name = "App2" };

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.UserGroups.Add(testGroup);

                context.App.Add(app1);
                context.App.Add(app2);

                testGroup.UserGroupsApps.Add(new UserGroupsApps { AppId = app1.Id, UserGroupId = testGroup.Id });
                testGroup.UserGroupsApps.Add(new UserGroupsApps { AppId = app2.Id, UserGroupId = testGroup.Id });

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
            authResult.Group.Should().Be("TestGroup");
            authResult.Apps.Count.Should().Be(2);
            authResult.Apps[0].Name.Should().Be(app1.Name);
            authResult.Apps[1].Name.Should().Be(app2.Name);
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
