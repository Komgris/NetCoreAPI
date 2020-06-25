using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineOperators
    {
        public int Id { get; set; }
        public string PlanId { get; set; }
        public int MachineId { get; set; }
        public int OperatorCount { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
