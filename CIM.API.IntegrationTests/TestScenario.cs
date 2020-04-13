using CIM.API.IntegrationTests.DbModel;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;


namespace CIM.API.IntegrationTests
{
    public class TestScenario
    {
        public HttpClient TestClient;
        public cim_dbContext DbContext;
        public IServiceScopeFactory ServiceScopeFactory;
        public UserModel Admin { get; set; }
        public string AdminToken { get; set; }
        public TestDbModel TestDb { get; set; } = new TestDbModel();
        public MasterDataModel MasterData { get; set; }
        public WebApplicationFactory<Startup> AppFactory;

    }
}
