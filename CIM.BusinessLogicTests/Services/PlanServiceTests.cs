using CIM.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using Moq;
using CIM.DAL.Interfaces;
using CIM.Model;
using CIM.BusinessLogic.Interfaces;
using System.Linq;
using CIM.Domain.Models;
using System.Text.Json;

namespace CIM.BusinessLogicTests.Services
{
    public class PlanServiceTests
    {

        [Fact]
        public void ListTest()
        {
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var responseCacheService = new Mock<IResponseCacheService>().Object;
            var masterDataService = new Mock<IMasterDataService>().Object;
            var planRepository = new Mock<IProductionPlanRepository>().Object;
            var productRepository = new Mock<IProductRepository>();
            var machineService = new Mock<IMachineService>();
            var activeProcessService = new Mock<IActiveProductionPlanService>();
            var recordManufacturingLossService = new Mock<IRecordManufacturingLossService>();

            var planService = new ProductionPlanService(
                responseCacheService,
                masterDataService,
                unitOfWork,
                planRepository,
                productRepository.Object,
                machineService.Object,
                activeProcessService.Object,
                recordManufacturingLossService.Object
            );
            //var result = planService.List();
            //result.Should().NotBeNull();
        }
        [Fact]
        public void ComparePlan()
        {
            string path = @"D:\PSEC\Dole\Doc\test.xlsx";

            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var responseCacheService = new Mock<IResponseCacheService>().Object;
            var masterDataService = new Mock<IMasterDataService>().Object;
            var planRepository = new Mock<IProductionPlanRepository>().Object;
            var productRepository = new Mock<IProductRepository>();
            var machineService = new Mock<IMachineService>();
            var activeProcessService = new Mock<IActiveProductionPlanService>();
            var recordManufacturingLossService = new Mock<IRecordManufacturingLossService>();

            var planService = new ProductionPlanService(
                responseCacheService,
                masterDataService,
                unitOfWork,
                planRepository,
                productRepository.Object,
                machineService.Object,
                activeProcessService.Object,
                recordManufacturingLossService.Object
            );

            var dbPlanMoq = new List<ProductionPlan>()
            {
                new ProductionPlan{ PlanId = "1" },
                new ProductionPlan{ PlanId = "2" },
                new ProductionPlan{ PlanId = "13" },
                new ProductionPlan{ PlanId = "14" },
            };

            //planRepository.Setup(x => x.All())
            //    .Returns(dbPlanMoq.AsQueryable());
            //var productionPlan = new List<ProductionPlanModel>();
            //var dbPlan = planService.List();
            //dbPlan.Any(x => string.IsNullOrEmpty( x.PlanId)).Should().Be(false);
            //dbPlan.Should().NotBeNull();
            //var list = planService.ReadImport(path);
            //var result = planService.Compare(list, dbPlan);
            //var json = JsonSerializer.Serialize(result);
            //result.Count(x => x.IsDuplicate == true).Should().Be(2);
        }
        [Fact]
        public void ImportTest()
        {
            string path = @"D:\PSEC\Dole\Doc\test.xlsx";
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var responseCacheService = new Mock<IResponseCacheService>().Object;
            var masterDataService = new Mock<IMasterDataService>().Object;
            var planRepository = new Mock<IProductionPlanRepository>().Object;
            var productRepository = new Mock<IProductRepository>();
            var machineService = new Mock<IMachineService>();
            var activeProcessService = new Mock<IActiveProductionPlanService>();
            var recordManufacturingLossService = new Mock<IRecordManufacturingLossService>();


            var planService = new ProductionPlanService(
                responseCacheService,
                masterDataService,
                unitOfWork,
                planRepository,
                productRepository.Object,
                machineService.Object,
                activeProcessService.Object,
                recordManufacturingLossService.Object
            );
            //var result = planService.ReadImport(path);
            //result.Should().NotBeNull();
        }
        [Fact]
        public void Insert()
        {
            var dbPlanMoq = new List<ProductionPlanModel>()
            {
                new ProductionPlanModel{ PlanId = "51" },
                new ProductionPlanModel{ PlanId = "52" },
                new ProductionPlanModel{ PlanId = "53" },
                new ProductionPlanModel{ PlanId = "54" },
            };
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var responseCacheService = new Mock<IResponseCacheService>().Object;
            var masterDataService = new Mock<IMasterDataService>().Object;
            var planRepository = new Mock<IProductionPlanRepository>().Object;
            var productRepository = new Mock<IProductRepository>();
            var machineService = new Mock<IMachineService>();
            var activeProcessService = new Mock<IActiveProductionPlanService>();
            var recordManufacturingLossService = new Mock<IRecordManufacturingLossService>();

            var planService = new ProductionPlanService(
                responseCacheService,
                masterDataService,
                unitOfWork,
                planRepository,
                productRepository.Object,
                machineService.Object,
                activeProcessService.Object,
                recordManufacturingLossService.Object
            );
            //var result = planService.Insert(dbPlanMoq);
            //result.Should().BeTrue();
        }
    }
}
