using System.Collections.Generic;

namespace CIM.Model
{
    public class ActiveMachineModel
    {
        public int Id { get; set; }

        public string ProductionPlanId { get; set; }
        public List<int> RouteIds { get; set; } 

        public Dictionary<int, MachineComponentModel> ComponentList { get; set; }
        public int StatusId { get; set; }
    }
}