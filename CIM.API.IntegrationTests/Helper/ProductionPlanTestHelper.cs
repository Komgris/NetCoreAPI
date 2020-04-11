using CIM.Model;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace CIM.API.IntegrationTests.Helper
{
    public static class ProductionPlanTestHelper
    {
        public static List<ProductionPlanModel> GetProductionPlansList()
        {
            return new List<ProductionPlanModel>()
            {
                new ProductionPlanModel{ PlanId="testA",ProductId=1,Status="new"},
                new ProductionPlanModel{ PlanId="testB",ProductId=2,Status="new"},
                new ProductionPlanModel{ PlanId="testC",ProductId=2,Status="new"},
            };
        }

        public static void CompareModelProductionPlan(List<ProductionPlanModel> model, ProductionPlanModel expect)
        {
            var compareList = model.First(x => x.PlanId == expect.PlanId);
            compareList.PlanId.Should().Be(expect.PlanId);
            compareList.ProductId.Should().Be(expect.ProductId);
        }


    }
}
