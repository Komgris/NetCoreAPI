using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordProductionPlanOutput
    {
        public int Id { get; set; }
        public string ProductionPlanId { get; set; }
        public int? MachineId { get; set; }
        public bool? IsCounterOut { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public string Remark { get; set; }
        public int WeekNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
