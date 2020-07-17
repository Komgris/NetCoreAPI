using System;
using System.Collections.Generic;

namespace CIM.Model
{
    public class ActiveMachineModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Image { get; set; }
        public string ProductionPlanId { get; set; }
        public List<int> RouteIds { get; set; } = new List<int>();

        public Dictionary<int, MachineComponentModel> ComponentList { get; set; }
        public int StatusId { get; set; }
        public bool IsReady { get; set; }
        public string UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public RecordProductionPlanOutputModel RecordProductionPlanOutput { get; set; }
    }
}