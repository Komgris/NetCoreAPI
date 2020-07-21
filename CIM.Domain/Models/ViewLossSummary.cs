using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ViewLossSummary
    {
        public int? MachineId { get; set; }
        public string ProductionPlanId { get; set; }
        public int RouteId { get; set; }
        public int LossLevel1Id { get; set; }
        public int LossLevel2Id { get; set; }
        public int LossLevel3Id { get; set; }
        public long? ElapseSec { get; set; }
    }
}
