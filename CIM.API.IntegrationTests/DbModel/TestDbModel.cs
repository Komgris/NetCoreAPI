using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.API.IntegrationTests.DbModel
{
    public class TestDbModel
    {

        public List<Machine> MachineDb = new List<Machine>
        {
            new Machine { Id = 1, Name = "TestMC1" },
        };

        public List<Product> ProductsDb { get; set; } = new List<Product>
        {
            new Product { Id = 1, Code = "TestProduct1" },
        };

        public List<Route> RoutesDb { get; set; } = new List<Route>
        {
            new Route { Id = 1, Name = "TestRoute01" },
            new Route { Id = 2, Name = "TestRoute02" },
        };
        public List<ProductionPlan> ProductionPlansDb { get; set; } = new List<ProductionPlan>
        {
            new ProductionPlan { PlanId = "IT_PP_START_01" },
            new ProductionPlan { PlanId = "IT_PP_START_02" },
        };


    }
}
