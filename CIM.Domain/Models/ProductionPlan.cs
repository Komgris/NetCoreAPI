using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductionPlan
    {
        public ProductionPlan()
        {
            RecordProductionPlanColorOrder = new HashSet<RecordProductionPlanColorOrder>();
            RecordProductionPlanInformation = new HashSet<RecordProductionPlanInformation>();
        }

        public int Id { get; set; }
        public string PlanId { get; set; }
        public int ProductId { get; set; }
        public int MachineId { get; set; }
        public string ShopNo { get; set; }
        public double Target { get; set; }
        public decimal? Sequence { get; set; }
        public int? UnitId { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? PlanFinish { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualFinish { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public decimal? Standard { get; set; }

        public virtual ICollection<RecordProductionPlanColorOrder> RecordProductionPlanColorOrder { get; set; }
        public virtual ICollection<RecordProductionPlanInformation> RecordProductionPlanInformation { get; set; }
    }
}
