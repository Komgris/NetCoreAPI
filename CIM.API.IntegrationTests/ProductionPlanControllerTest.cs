﻿using CIM.Model;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Xunit;
using Newtonsoft.Json;
using CIM.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;

namespace CIM.API.IntegrationTests
{
    public  class ProductionPlanControllerTest : IntegrationTest
    {

        [Fact]
        public async Task Load_Test()
        {
            var productionPlan = new ProductionPlanModel();
            var testRouteId = 123;
            var orgUpdateDate = new DateTime(2000, 1, 1);
            var moqProductionPlan = new ProductionPlan
            {
                PlanId = "1",
                ProductId = 123,
                UpdatedAt = orgUpdateDate,
                CreatedBy = "OrgCreatedBy",
                CreatedAt = new DateTime(2000, 1, 1)
        };
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(moqProductionPlan);
                context.SaveChanges();
            }
            productionPlan.PlanId = moqProductionPlan.PlanId;
            productionPlan.RouteId = testRouteId;

            // Act
            var content = GetHttpContentForPost(productionPlan, AdminToken);
            var loadResponse = await TestClient.PostAsync("api/ProductionPlan/Load", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/ProductionPlan/{productionPlan.PlanId}");
            request.Headers.Add("token", AdminToken);
            var getResponse = await TestClient.SendAsync(request);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>((await getResponse.Content.ReadAsStringAsync()));
            getResult.Data.PlanId.Should().Equals(productionPlan.PlanId);
            getResult.Data.RouteId.Should().Equals(productionPlan.RouteId);
            getResult.Data.ProductId.Should().Equals(productionPlan.ProductId);
            getResult.Data.ActualFinish.Should().Equals(productionPlan.ActualFinish);
            getResult.Data.ActualStart.Should().Equals(productionPlan.ActualStart);
            getResult.Data.ActualStart.Should().Equals(productionPlan.ActualStart);
            getResult.Data.CreatedBy.Should().Equals(moqProductionPlan.CreatedBy);
            getResult.Data.CreatedAt.Should().Equals(moqProductionPlan.CreatedAt);
            getResult.Data.UpdatedBy.Should().Equals(Admin.Id);
            (getResult.Data.UpdatedAt != null).Should().BeTrue();
            (getResult.Data.UpdatedAt != orgUpdateDate).Should().BeTrue();
        }

        [Fact]
        public async Task Load_WhenProductionPlanStarted_Test()
        {
            var productionPlan = new ProductionPlanModel();
            var testRouteId = 123;
            var moqDbModel = new ProductionPlan
            {
                PlanId = "WhenProductionPlanStarted",
                ProductId = 123,
                Status = Constans.PRODUCTION_PLAN_STATUS.STARTED,
            };
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(moqDbModel);
                context.SaveChanges();
            }

            productionPlan.PlanId = moqDbModel.PlanId;
            productionPlan.RouteId = testRouteId;

            // Act
            var content = GetHttpContentForPost(productionPlan, AdminToken);
            var loadResponse = await TestClient.PostAsync("api/ProductionPlan/Load", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>((await loadResponse.Content.ReadAsStringAsync()));
            result.IsSuccess.Should().Equals(false);
            result.Message.Should().StartWith($"System.Exception: {ErrorMessages.PRODUCTION_PLAN.PLAN_STARTED}");
        }

        [Fact]
        public async Task List_Test()
        {
            var productionPlanModel = new ProductionPlan
            {
                PlanId = "Testcode",
                ProductId = 123,
                Status = Constans.PRODUCTION_PLAN_STATUS.STARTED,
                IsActive = true
            };
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(productionPlanModel);
                context.SaveChanges();
            }

            // Act
            var content = GetHttpContentForPost(productionPlanModel, AdminToken);
            var loadResponse = await TestClient.PostAsync("api/ProductionPlan/Get/1/1", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>((await loadResponse.Content.ReadAsStringAsync()));
            result.IsSuccess.Should().Equals(false);
            result.Message.Should().StartWith($"System.Exception: {ErrorMessages.PRODUCTION_PLAN.PLAN_STARTED}");
        }
    }
}
