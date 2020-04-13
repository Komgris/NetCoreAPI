using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductionPlan
    {
        public ProductionPlan()
        {
            RecordMachineComponentLoss = new HashSet<RecordMachineComponentLoss>();
            RecordManufacturingLoss = new HashSet<RecordManufacturingLoss>();
            RecordProductionPlanOutput = new HashSet<RecordProductionPlanOutput>();
        }

        public string PlanId { get; set; }
        public int ProductId { get; set; }
        public int? RouteId { get; set; }
        public int? Target { get; set; }
        public int? UnitId { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? PlanFinish { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualFinish { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
        public virtual ICollection<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
        public virtual ICollection<RecordProductionPlanOutput> RecordProductionPlanOutput { get; set; }
    }
}
