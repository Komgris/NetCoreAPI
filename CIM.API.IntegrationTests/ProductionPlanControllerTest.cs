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

namespace CIM.API.IntegrationTests
{
    public  class ProductionPlanControllerTest : IntegrationTest
    {

        [Fact(Skip = "Need to change bl")]
        public async Task Start_Test()
        {
            var productionPlan = new ProductionPlanModel();
            var orgUpdateDate = new DateTime(2000, 1, 1);
            var moqProductionPlan = new ProductionPlan
            {
                PlanId = "1",
                UpdatedAt = orgUpdateDate,
                CreatedBy = "OrgCreatedBy",
                CreatedAt = new DateTime(2000, 1, 1)
            };

            var product = new Product
            {
                BriteItemPerUpcitem = "",
                Code = "P001",
                CreatedAt = new DateTime(),
                CreatedBy = "OrgCreatedBy",
                Description = "",
                IsActive = true,
                Igweight = 100,
            };

            var machineType = new MachineType
            {
                CreatedAt = new DateTime(),
                CreatedBy = "OrgCreatedBy",
                IsActive = true,
                Name = "MachineType01"
            };

            var productGroup = new ProductGroup
            {
                IsActive = true,
                CreatedAt = new DateTime(),
                CreatedBy = "OrgCreatedBy",
                Name = "PG1",
            };

            var route = new Route { 
                Name = "r01",
                CreatedAt = new DateTime(),
                CreatedBy = "OrgCreatedBy",
                IsActive = true,
            };
            var routeMachine = new RouteMachine
            {
                CreatedAt = new DateTime(),
                CreatedBy = "OrgCreatedBy",
                IsActive = true,
                Machine = new Machine
                {
                    CreatedAt = new DateTime(),
                    CreatedBy = "OrgCreatedBy",
                    IsActive = true,
                    MachineType = machineType

                }
            };
            route.RouteMachine.Add(routeMachine);
            var routeProductGroup = new RouteProductGroup
            {
                CreatedAt = new DateTime(),
                CreatedBy = "OrgCreatedBy",
                IsActive = true,
                Route = route,
            };
            routeProductGroup.RouteId = route.Id;
            productGroup.Product.Add(product);
            productGroup.RouteProductGroup.Add(routeProductGroup);
            productionPlan.RouteId = route.Id;

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.RouteMachine.Add(routeMachine);
                context.ProductGroup.Add(productGroup);
                context.RouteProductGroup.Add(routeProductGroup);
                moqProductionPlan.ProductId = product.Id;
                context.ProductionPlan.Add(moqProductionPlan);
                context.SaveChanges();
            }

            await SendRefreshMasterData();

            productionPlan.PlanId = moqProductionPlan.PlanId;
            productionPlan.RouteId = route.Id;

            // Act
            var content = GetHttpContentForPost(productionPlan, AdminToken);
            var loadResponse = await TestClient.PostAsync("api/ProductionPlan/Start", content);
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

        [Fact(Skip = "Need to change bl")]
        public async Task Start_WhenProductionPlanStartedOnTheSameRoute_Test()
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
            var loadResponse = await TestClient.PostAsync("api/ProductionPlan/Start", content);
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<ProcessReponseModel<ProductionPlanModel>>((await loadResponse.Content.ReadAsStringAsync()));
            result.IsSuccess.Should().Equals(false);
            result.Message.Should().StartWith($"System.Exception: {ErrorMessages.PRODUCTION_PLAN.PLAN_STARTED}");
        }

        [Fact]
        public async Task GET_Test()
        {
            var productionPlanModel = new ProductionPlan
            {
                PlanId = "TestListcode",
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
            var loadResponse = await TestClient.GetAsync("api/ProductionPlan/Get/1/1");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<PagingModel<ProductionPlanModel>>((await loadResponse.Content.ReadAsStringAsync()));

            result.Data.Should().NotBeNull();
            result.Data.Where(x => x.PlanId == productionPlanModel.PlanId);
        }

        [Fact]
        public async Task List_Test()
        {
            var productionPlan = new ProductionPlan
            {
                PlanId = "TestListcode",
                ProductId = 123,
                Status = Constans.PRODUCTION_PLAN_STATUS.STARTED,
                IsActive = true
            };
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<cim_dbContext>();
                context.ProductionPlan.Add(productionPlan);
                context.SaveChanges();
            }

            // Act
            var content = GetHttpContentForPost(productionPlan, AdminToken);
            var loadResponse = await TestClient.GetAsync("api/ProductionPlans");
            loadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = JsonConvert.DeserializeObject<PagingModel<ProductionPlanListModel>>((await loadResponse.Content.ReadAsStringAsync()));

            result.Data.Should().NotBeNull();
            result.Data.Where(x => x.Id == productionPlan.PlanId);
        }
    }
}
