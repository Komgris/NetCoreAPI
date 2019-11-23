using CIM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
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
            var registerUserModel = new UserModel
            {
                Email = "test@email.com",
                UserName = "user1",
                Password = "super-secret",
                FirstName = "Hans",
                LastName = "Meier",
                LanguageId = "en",
                Image = null,

            };
            var testGroup = new UserGroups
            {
                IsActive = true,
                Name = "TestGroup"
            };

            var app1 = new App { IsActive = true, Name = "App1" };
            var app2 = new App { IsActive = true, Name = "App2" };
            var app3 = new App { IsActive = false, Name = "App3" };
            var app4 = new App { IsActive = true, Name = "App4" };
            var token = string.Empty;

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.UserGroups.Add(testGroup);
                context.App.Add(app1);
                context.App.Add(app2);
                context.App.Add(app3);
                context.App.Add(app4);

                testGroup.UserGroupsApps.Add(new UserGroupsApps { AppId = app1.Id, UserGroupId = testGroup.Id });
                testGroup.UserGroupsApps.Add(new UserGroupsApps { AppId = app2.Id, UserGroupId = testGroup.Id });
                testGroup.UserGroupsApps.Add(new UserGroupsApps { AppId = app3.Id, UserGroupId = testGroup.Id });

                context.SaveChanges();
                registerUserModel.UserGroupId = testGroup.Id;

                token = AdminToken;
            }

            // Act
            var content = GetHttpContentForPost(registerUserModel, token);
            var response = await TestClient.PostAsync("api/User", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authResp = await TestClient.GetAsync($"api/Auth?username={registerUserModel.UserName}&password={registerUserModel.Password}");
            var authResult = JsonConvert.DeserializeObject<AuthModel>((await authResp.Content.ReadAsStringAsync()));
            // Assert
            authResp.StatusCode.Should().Be(HttpStatusCode.OK);
            authResult.UserId.Should().NotBeNullOrEmpty();
            authResult.IsSuccess.Should().BeTrue();
            authResult.Group.Should().Be("TestGroup");
            authResult.Token.Should().NotBeNullOrEmpty();
            authResult.Apps.Count.Should().Be(2);
            authResult.Apps[0].Name.Should().Be(app1.Name);
            authResult.Apps[1].Name.Should().Be(app2.Name);
            authResult.FullName.Should().Be(registerUserModel.FirstName + " " + registerUserModel.LastName);
        }


        [Fact]
        public async Task List_Test()
        {
            var response = await TestClient.GetAsync($"User");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = JsonConvert.DeserializeObject<List<UserModel>>((await response.Content.ReadAsStringAsync()));
        }
    }
}
