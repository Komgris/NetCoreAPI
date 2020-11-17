using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public partial class ProductionPlan3MModel
    {
        public string PlanId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int MachineId { get; set; }
        public string ShopNo { get; set; }
        public int Target { get; set; }
        public decimal? Sequence { get; set; }
        public int? UnitId { get; set; }
        public int CompareResult { get; set; }
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
    }
}
