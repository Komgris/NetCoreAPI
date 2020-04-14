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
using System;
using CIM.API.IntegrationTests.Helper;
using System.Collections.Generic;
using System.Net.Http.Headers;
using CIM.BusinessLogic.Interfaces;
using CIM.API.IntegrationTests.DbModel;
using Microsoft.EntityFrameworkCore;

namespace CIM.API.IntegrationTests
{
    public class ProductionPlanControllerTest : BaseIntegrationTest
    {
        TestScenario scenario;

        public ProductionPlanControllerTest()
        {
            //Run on each test
            scenario = CreateWebApplication();
            Setup(scenario);
        }

        void Setup(TestScenario seacario)
        {

            var product = seacario.TestDb.ProductsDb[0];
            var route = seacario.TestDb.RoutesDb[0];
            var productionPlanTest = seacario.TestDb.ProductionPlansDb[0];

            CleanDb(seacario);

            //Start_Test
            using (var scope = seacario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(productionPlanTest);
                context.Product.Add(product);
                context.Route.Add(route);
                context.SaveChanges();
            }

        }

        public int CountProductionPlan(TestScenario scenario)
        {
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                return context.ProductionPlan.Count();
            }
        }

        public ProductionPlan Get(string id, TestScenario scenario)
        {
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                return context.ProductionPlan.First(x => x.PlanId == id);
            }
        }


        [Fact(Skip = "Need to Fix later")]
        public async Task Start_Test()
        {

            var productionPlan = scenario.TestDb.ProductionPlansDb[0];
            var product = scenario.TestDb.ProductsDb[0];
            var route = scenario.TestDb.RoutesDb[0];

            var testProductionPlan = new ProductionPlanModel
            {
                PlanId = productionPlan.PlanId,
                ProductId = product.Id,
                RouteId = route.Id
            };

            var content = GetHttpContentForPost(testProductionPlan, scenario.AdminToken);
            var loadResponse = await scenario.TestClient.PostAsync("api/ProductionPlanStart", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/ProductionPlan?id={testProductionPlan.PlanId}");
            request.Headers.Add("token", scenario.AdminToken);
            var getResponse = await scenario.TestClient.SendAsync(request);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>((await getResponse.Content.ReadAsStringAsync()));
            getResult.Data.PlanId.Should().Equals(productionPlan.PlanId);
            getResult.Data.ActualFinish.Should().Equals(productionPlan.ActualFinish);
            getResult.Data.PlanStart.Should().NotBeNull();
            getResult.Data.ActualStart.Should().NotBeNull();
            getResult.Data.UpdatedBy.Should().NotBeNull();
            getResult.Data.UpdatedAt.Should().NotBeNull();
            getResult.Data.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production;

        }

        [Fact]
        public async Task Start_WhenProductionPlanStartedOnTheSameRoute_Test()
        {

            var productionPlan = scenario.TestDb.ProductionPlansDb[0];
            var product = scenario.TestDb.ProductsDb[0];
            var route = scenario.TestDb.RoutesDb[0];

            var testProductionPlan = new ProductionPlanModel
            {
                PlanId = productionPlan.PlanId,
                ProductId = product.Id,
                RouteId = route.Id
            };

            // Act
            var content = GetHttpContentForPost(testProductionPlan, scenario.AdminToken);
            await scenario.TestClient.PostAsync("api/ProductionPlanStart", content);

            var loadResponse = await scenario.TestClient.PostAsync("api/ProductionPlanStart", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>((await loadResponse.Content.ReadAsStringAsync()));
            result.IsSuccess.Should().Equals(false);
            result.Message.Should().Be($"{ErrorMessages.PRODUCTION_PLAN.PLAN_STARTED}");
        }

        [Fact(Skip = "Need to Fix later")]
        public async Task GET_Test()
        {
            var productionPlanModel = new ProductionPlan
            {
                PlanId = "GET_Test0001",
                ProductId = 123,
                StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production,
                IsActive = true
            };
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(productionPlanModel);
                context.SaveChanges();
            }

            // Act
            var content = GetHttpContentForPost(productionPlanModel, scenario.AdminToken);
            var loadResponse = await scenario.TestClient.GetAsync($"api/ProductionPlan?id={productionPlanModel.PlanId}");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loadResponseString = await loadResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>(loadResponseString);

            result.Data.Should().NotBeNull();
            result.Data.PlanId.Should().Be(productionPlanModel.PlanId);
        }

        [Fact]
        public async Task List_Test()
        {
            var productionPlan = new ProductionPlan
            {
                PlanId = "TestListcode",
                ProductId = 123,
                StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production,
                IsActive = true
            };
            using (var scope = scenario.ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(productionPlan);
                context.SaveChanges();
            }

            // Act
            var content = GetHttpContentForPost(productionPlan, scenario.AdminToken);
            var loadResponse = await scenario.TestClient.GetAsync("api/ProductionPlans");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loadResponseString = await loadResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<PagingModel<ProductionPlanListModel>>>(loadResponseString);

            result.Data.Should().NotBeNull();
            result.Data.Data.Where(x => x.Id == productionPlan.PlanId);
        }

        [Fact]
        public async Task InsertProductionPlan_Test()
        {
            var dbProductionPlanMoq = new List<ProductionPlanModel>()
            {
                new ProductionPlanModel{ PlanId="testA",ProductId=1,StatusId=(int)Constans.PRODUCTION_PLAN_STATUS.New},
                new ProductionPlanModel{ PlanId="testB",ProductId=2,StatusId=(int)Constans.PRODUCTION_PLAN_STATUS.New},
                new ProductionPlanModel{ PlanId="testC",ProductId=2,StatusId=(int)Constans.PRODUCTION_PLAN_STATUS.New},
            };
            int countBeforeInsert = CountProductionPlan(scenario);

            var content = GetHttpContentForPost(dbProductionPlanMoq, scenario.AdminToken);
            var createResponse = await scenario.TestClient.PostAsync("/api/ProductionPlan/Create", content);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            int countAfterInsert = CountProductionPlan(scenario);
            var totalList = countAfterInsert - countBeforeInsert;
            totalList.Should().Be(dbProductionPlanMoq.Count());

            foreach (var expect in dbProductionPlanMoq)
            {
                var result = Get(expect.PlanId, scenario);
                result.PlanId.Should().Be(expect.PlanId);
            }
        }

    }
}
