using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model
{
    public class ActiveProductionPlan3MModel : BaseJSONModel
    {
        public ActiveProductionPlan3MModel(string planId)
        {
            ProductionPlanId = planId;
        }
        public string ProductionPlanId { get; set; }
        public int machineId { get; set; }
        public PRODUCTION_PLAN_STATUS Status { get; set; }
        public ProductionDataModel ProductionData { get; set; }        
    }
}
