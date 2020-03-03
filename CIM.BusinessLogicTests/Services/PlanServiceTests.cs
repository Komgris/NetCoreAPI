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
            var planRepository = new Mock<IProductionPlanRepository>().Object;
            var planService = new ProductionPlanService(unitOfWork, planRepository);
            //var result = planService.List();
            //result.Should().NotBeNull();
        }
        [Fact]
        public void ComparePlan()
        {
            string path = @"D:\PSEC\Dole\Doc\test.xlsx";
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var planRepository = new Mock<IProductionPlanRepository>();
            var dbPlanMoq = new List<ProductionPlan>()
            {
                new ProductionPlan{  PlantId = "1" },
                new ProductionPlan{  PlantId = "2" },
                new ProductionPlan{  PlantId = "13" },
                new ProductionPlan{  PlantId = "14" },
            };

            planRepository.Setup(x => x.All())
                .Returns(dbPlanMoq.AsQueryable());
            var planService = new ProductionPlanService(unitOfWork,planRepository.Object);
            var productionPlan = new List<ProductionPlanModel>();
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
            var planRepository = new Mock<IProductionPlanRepository>().Object;
            var planService = new ProductionPlanService(unitOfWork, planRepository);
            //var result = planService.ReadImport(path);
            //result.Should().NotBeNull();
        }
        [Fact]
        public void Insert()
        {
            var dbPlanMoq = new List<ProductionPlanModel>()
            {
                new ProductionPlanModel{ PlantId = "51" },
                new ProductionPlanModel{ PlantId = "52" },
                new ProductionPlanModel{ PlantId = "53" },
                new ProductionPlanModel{ PlantId = "54" },
            };
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var planRepository = new Mock<IProductionPlanRepository>().Object;
            var planService = new ProductionPlanService(unitOfWork, planRepository);
            //var result = planService.Insert(dbPlanMoq);
            //result.Should().BeTrue();
        }
    }
}
