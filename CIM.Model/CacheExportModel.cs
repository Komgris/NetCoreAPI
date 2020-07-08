using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class CacheExportModel
    {
        public List<ActiveProductionPlanModel> ProductionPlans { get; set; } = new List<ActiveProductionPlanModel>();

        public List<ActiveMachineModel> Machines { get; set; } = new List<ActiveMachineModel>();
    }
}
