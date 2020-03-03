using CIM.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIM.API.IntegrationTests.Helper
{
    public static class ProductionPlanTestHelper
    {
        public static List<ProductionPlanModel> GetProductionPlansList()
        {
            return new List<ProductionPlanModel>()
            {
                new ProductionPlanModel{ PlantId="testA",ProductId=1,Status="new"},
                new ProductionPlanModel{ PlantId="testB",ProductId=2,Status="new"},
                new ProductionPlanModel{ PlantId="testC",ProductId=2,Status="new"},
            };
        }

        public static void CompareModelProductionPlan(List<ProductionPlanModel> model, ProductionPlanModel expect)
        {
            var compareList = model.First(x => x.PlantId == expect.PlantId);
            compareList.PlantId.Should().Be(expect.PlantId);
            compareList.ProductId.Should().Be(expect.ProductId);
        }
    }
}
