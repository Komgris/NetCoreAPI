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
        public void CalculateTest()
        {
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var planRepository = new Mock<IPlanRepository>().Object;
            var planService = new PlanService(unitOfWork,planRepository);
            var result = planService.Plus(1, 1);
            result.Should().Be(2);
        }
        [Fact]
        public void ListTest()
        {
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var planRepository = new Mock<IPlanRepository>().Object;
            var planService = new PlanService(unitOfWork, planRepository);
            var result = planService.List();
            result.Should().NotBeNull();
        }
        [Fact]
        public void ComparePlan()
        {
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var planRepository = new Mock<IPlanRepository>();
            var dbPlanMoq = new List<ProductionPlan>()
            {
                new ProductionPlan{ Id = 1, PlantId = "1" },
                new ProductionPlan{ Id = 2, PlantId = "2" },
                new ProductionPlan{ Id = 3, PlantId = "13" },
                new ProductionPlan{ Id = 4, PlantId = "14" },
            };

            planRepository.Setup(x => x.All())
                .Returns(dbPlanMoq.AsQueryable());
            var planService = new PlanService(unitOfWork,planRepository.Object);
            var productionPlan = new List<ProductionPlanModel>();
            var dbPlan = planService.List();
            dbPlan.Any(x => string.IsNullOrEmpty( x.PlanId)).Should().Be(false);
            dbPlan.Should().NotBeNull();
            var list = planService.ReadImport();
            var result = planService.Compare(list, dbPlan);
            //var json = JsonSerializer.Serialize(result);
            result.Count(x => x.IsDuplicate == true).Should().Be(2);
        }
        [Fact]
        public void ImportTest()
        {
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var planRepository = new Mock<IPlanRepository>().Object;
            var planService = new PlanService(unitOfWork, planRepository);
            var result = planService.ReadImport();
            result.Should().NotBeNull();
        }
    }
}
