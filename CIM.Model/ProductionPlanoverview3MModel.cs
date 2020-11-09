using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanoverview3MModel
    {
        public ProductionPlanModel ProductionPlan { get; set; }

        public ActiveProcess3MModel Route { get; set; }
    }
}
