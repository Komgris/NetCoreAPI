using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordProductionPlanWasteNonePrime
    {
        public int Id { get; set; }
        public int WasteNonePrimeId { get; set; }
        public string ProductionPlanPlanId { get; set; }
        public int RouteId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual ProductionPlan ProductionPlanPlan { get; set; }
        public virtual Route Route { get; set; }
        public virtual WasteNonePrime WasteNonePrime { get; set; }
    }
}
