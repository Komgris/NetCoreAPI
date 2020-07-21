using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ViewProduceCounterCase
    {
        public int Id { get; set; }
        public string ProductionPlanId { get; set; }
        public int? MachineId { get; set; }
        public int? CounterIn { get; set; }
        public int? TotalIn { get; set; }
        public int? CounterOut { get; set; }
        public int? TotalOut { get; set; }
        public bool? GoalByHour { get; set; }
        public string Remark { get; set; }
        public int? Hour { get; set; }
        public int WeekNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? FactorDivide { get; set; }
        public int? FactorMultiply { get; set; }
        public int? CounterInCase { get; set; }
        public int? TotalInCase { get; set; }
        public int? CounterOutCase { get; set; }
        public int? TotalOutCase { get; set; }
    }
}
