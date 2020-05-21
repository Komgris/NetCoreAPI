using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanOverviewModel
    {
        public ProductionPlanModel ProductionPlan { get; set; }

        public ActiveProcessModel Route { get; set; }
    }
}
