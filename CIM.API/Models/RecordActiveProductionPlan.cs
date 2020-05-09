using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class RecordActiveProductionPlan
    {
        public int Id { get; set; }
        public string ProductionPlanPlanId { get; set; }
        public int RouteId { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public DateTime? EstimateFinish { get; set; }
        public int? StatusId { get; set; }
        public int Target { get; set; }
        public string CreatedBy { get; set; }
        public int? OperatorSetId { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual ProductionPlan ProductionPlanPlan { get; set; }
        public virtual Route Route { get; set; }
        public virtual ProductionStatus Status { get; set; }
    }
}
