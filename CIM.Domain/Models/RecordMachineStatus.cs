using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordMachineStatus
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public int MachineStatusId { get; set; }
        public string ProductionPlanId { get; set; }
        public int? Hour { get; set; }
        public int? WeekNumber { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int TimeSpan { get; set; }
    }
}
