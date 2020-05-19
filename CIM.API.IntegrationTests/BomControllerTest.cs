using CIM.Model;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using System.Linq;
using Xunit;
using Newtonsoft.Json;
using CIM.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Collections.Generic;

namespace CIM.API.IntegrationTests
{
    public class BomControllerTest : BaseIntegrationTest
    {
        TestScenario scenario;

        public BomControllerTest()
        {
            scenario = CreateWebApplication();
        }

        public BomTemp Get(string name, TestScenario scenario)
        {
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                return context.BomTemp.First(x => x.Name == name);
            }
        }

        [Fact]
        public async Task List_Test()
        {
            var bomList = new List<BomTemp>()
            {
                new BomTemp{ Name="testA",IsActive=true},
                new BomTemp{ Name="testB",IsActive=true},
                new BomTemp{ Name="testC",IsActive=true},
                new BomTemp{ Name="testD",IsActive=true},
            };

            foreach (var model in bomList)
            {
                using (var scope = scenario.ServiceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<cim_dbContext>();
                    context.BomTemp.Add(model);
                    context.SaveChanges();
                }
            }

            // Act
            var loadResponse = await scenario.TestClient.GetAsync($"api/Bom/List?page=1&howmany={bomList.Count()}");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loadResponseString = await loadResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<PagingModel<BomModel>>>(loadResponseString);

            result.Data.Should().NotBeNull();
            result.Data.Data.Count().Should().Be(4);
            result.Data.Data.Where(x => x.Name == bomList[0].Name);
        }

    }
}
