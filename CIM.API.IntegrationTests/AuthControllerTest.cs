using CIM.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net;

namespace CIM.API.IntegrationTests
{
    public class AuthControllerTest : IntegrationTest
    {

        [Fact]
        public async Task Auth_WhenLoginWithWrongUser_Test()
        {
            var authResp = await TestClient.GetAsync($"api/Auth?username=unknow&password=unknow");
            var authResult = JsonConvert.DeserializeObject<AuthModel>((await authResp.Content.ReadAsStringAsync()));
            authResult.UserId.Should().BeNull();
            authResult.IsSuccess.Should().BeFalse();
            authResult.FullName.Should().BeNull();
        }

        [Fact]
        public async Task Auth_LoginAsAdmin_Test()
        {
            var authResp = await TestClient.GetAsync($"api/Auth?username={Admin.UserName}&password={Admin.Password}");
            var authResult = JsonConvert.DeserializeObject<AuthModel>((await authResp.Content.ReadAsStringAsync()));
            authResp.StatusCode.Should().Be(HttpStatusCode.OK);
            authResult.UserId.Should().NotBeNullOrEmpty();
            authResult.IsSuccess.Should().BeTrue();
            authResult.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Auth_LoginAsAdminWithExitingToken_Test()
        {
            await TestClient.GetAsync($"api/Auth?username={Admin.UserName}&password={Admin.Password}");
            var authResp = await TestClient.GetAsync($"api/Auth?username={Admin.UserName}&password={Admin.Password}");
            var authResult = JsonConvert.DeserializeObject<AuthModel>((await authResp.Content.ReadAsStringAsync()));
            authResult.UserId.Should().NotBeNullOrEmpty();
            authResult.IsSuccess.Should().BeTrue();
            authResult.Token.Should().NotBeNullOrEmpty();
        }

    }
}
