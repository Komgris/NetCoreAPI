using System.Collections.Generic;

namespace CIM.Model
{
    public class ActiveMachineModel
    {
        public int Id { get; set; }

        public string ProductionPlanId { get; set; }
        public int[] RouteIds { get; set; } 

        public Dictionary<int, MachineComponentModel> ComponentList { get; set; }
    }
}