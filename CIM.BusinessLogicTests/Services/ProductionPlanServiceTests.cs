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
using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using CIM.BusinessLogic.Utility;

namespace CIM.BusinessLogic.Services.Tests
{
    public class ProductionPlanServiceTests
    {



        [Fact]
        public async Task Start_WhenProductionPlanNeverStartedBeforeTest()
        {

            var routeId = 1;
            var productionPlanId = "moqPP1";
            var productId = 123;
            var machineId = 456;

            //moq master data
            var masterDataService = new Mock<IMasterDataService>();
            var masterDataMoq = new MasterDataModel
            {
                Routes = new Dictionary<int, RouteModel>()
            };
            var machineList = new Dictionary<int, MachineModel>();
            machineList.Add(machineId, new MachineModel { Id = machineId });
            masterDataMoq.Routes.Add(routeId, new RouteModel { Id = routeId, MachineList = machineList });
            masterDataService.Setup(x => x.GetData() ).Returns(Task.FromResult(masterDataMoq));

            var responseCacheService = new Mock<IResponseCacheService>();

            var unitOfWork = new Mock<IUnitOfWorkCIM>();

            // return production plan that haven't started yet
            var productionPlanRepository = new Mock<IProductionPlanRepository>();
            var productionPlanMoq = new ProductionPlan { PlanId = productionPlanId, Status = null };
            productionPlanRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductionPlan, bool>>>())).Returns(Task.FromResult(productionPlanMoq));

            var productRepository = new Mock<IProductRepository>();

            var service = new ProductionPlanService(
                    responseCacheService.Object,
                    masterDataService.Object,
                    unitOfWork.Object,
                    productionPlanRepository.Object,
                    productRepository.Object
            );
            service.CurrentUser = new CurrentUserModel { UserId = "UnitTest1" };
            var productionPlan = new ProductionPlanModel
            {
                RouteId = routeId,
                ProductId = productId,
                PlanId = productionPlanId,
            };
            var result = await service.Start(productionPlan);
            result.Should().NotBeNull();

            result.Route.Id = routeId;
            result.ProductionPlanId = productionPlanId;
            result.ProductId = productId;
            result.Route.MachineList.Count.Should().Be(masterDataMoq.Routes[routeId].MachineList.Count);
            result.Route.MachineList[machineId].Id.Should().Be(machineId);

        }

        [Fact]
        public async Task Start_WhenProductionPlanAlreadyStartedTest()
        {

            var existingRouteId = 1;
            var newRouteId = 2;
            var productionPlanId = "moqPP1";
            var productId = 123;
            var machineId = 456;

            //moq master data
            var masterDataService = new Mock<IMasterDataService>();
            var masterDataMoq = new MasterDataModel
            {
                Routes = new Dictionary<int, RouteModel>()
            };
            var machineList = new Dictionary<int, MachineModel>();
            machineList.Add(machineId, new MachineModel { Id = machineId });
            masterDataMoq.Routes.Add(existingRouteId, new RouteModel { Id = existingRouteId, MachineList = machineList });
            masterDataMoq.Routes.Add(newRouteId, new RouteModel { Id = newRouteId, MachineList = machineList });
            masterDataService.Setup(x => x.GetData()).Returns(Task.FromResult(masterDataMoq));

            //return a active process because this production plan already started
            var responseCacheService = new Mock<IResponseCacheService>();
            var activeProcessMoq = new ActiveProcessModel { ProductId = productId, ProductionPlanId = productionPlanId, Route = MapperHelper.AsModel(masterDataMoq.Routes[existingRouteId], new ActiveRouteModel()) };
            responseCacheService.Setup(x => x.GetAsTypeAsync<ActiveProcessModel>(It.IsAny<string>())).Returns(Task.FromResult(activeProcessMoq));

            var unitOfWork = new Mock<IUnitOfWorkCIM>();

            var productionPlanRepository = new Mock<IProductionPlanRepository>();
            var productionPlanMoq = new ProductionPlan { PlanId = productionPlanId, StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production };
            productionPlanRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductionPlan, bool>>>())).Returns(Task.FromResult(productionPlanMoq));
            
            var productRepository = new Mock<IProductRepository>();

            var service = new ProductionPlanService(
                    responseCacheService.Object,
                    masterDataService.Object,
                    unitOfWork.Object,
                    productionPlanRepository.Object,
                    productRepository.Object
            );
            service.CurrentUser = new CurrentUserModel { UserId = "UnitTest1" };
            var productionPlan = new ProductionPlanModel
            {
                RouteId = newRouteId,
                ProductId = productId,
                PlanId = productionPlanId,
            };
            var result = await service.Start(productionPlan);
            result.Should().NotBeNull();

            result.Route.Id = newRouteId;
            result.ProductionPlanId = productionPlanId;
            result.ProductId = productId;
            result.Route.MachineList.Count.Should().Be(masterDataMoq.Routes[existingRouteId].MachineList.Count);
            result.Route.MachineList[machineId].Id.Should().Be(machineId);

        }
    }
}