using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;

namespace CIM.API.IntegrationTests
{
    public class SiteControllerTests : IntegrationTest
    {

        [Fact]
        public async Task Get_Test()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("api/Site");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
