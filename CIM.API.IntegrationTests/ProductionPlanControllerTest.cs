﻿using CIM.Model;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using CIM.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace CIM.API.IntegrationTests
{
    public  class ProductionPlanControllerTest : IntegrationTest
    {

        [Fact]
        public async Task Load_Test()
        {
            var productionPlan = new ProductionPlanModel();
            var testRouteId = 123;
            var moqProductionPlan = new ProductionPlan
            {
                PlantId = "1",
                ProductId = 123
            };
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(moqProductionPlan);
                context.SaveChanges();
            }
            productionPlan.PlantId = moqProductionPlan.PlantId;
            productionPlan.RouteId = testRouteId;

            // Act
            var content = GetHttpContentForPost(productionPlan, AdminToken);
            var loadResponse = await TestClient.PostAsync("api/ProductionPlan/Load", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/ProductionPlan/{productionPlan.PlantId}");
            request.Headers.Add("token", AdminToken);
            var getResponse = await TestClient.SendAsync(request);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>((await getResponse.Content.ReadAsStringAsync()));
            getResult.Data.PlantId.Should().Equals(productionPlan.PlantId);
            getResult.Data.RouteId.Should().Equals(productionPlan.RouteId);
            getResult.Data.ProductId.Should().Equals(productionPlan.ProductId);
            getResult.Data.ActualFinish.Should().Equals(productionPlan.ActualFinish);
            getResult.Data.ActualStart.Should().Equals(productionPlan.ActualStart);
        }

        [Fact]
        public async Task Load_WhenProductionPlanStarted_Test()
        {
            var productionPlan = new ProductionPlanModel();
            var testRouteId = 123;
            var moqDbModel = new ProductionPlan
            {
                PlantId = "WhenProductionPlanStarted",
                ProductId = 123,
                Status = Constans.PRODUCTION_PLAN_STATUS.STARTED,
            };
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(moqDbModel);
                context.SaveChanges();
            }

            productionPlan.PlantId = moqDbModel.PlantId;
            productionPlan.RouteId = testRouteId;

            // Act
            var content = GetHttpContentForPost(productionPlan, AdminToken);
            var loadResponse = await TestClient.PostAsync("api/ProductionPlan/Load", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);


        }
    }
}
