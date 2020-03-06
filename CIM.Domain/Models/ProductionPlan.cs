using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductionPlan
    {
        public string PlantId { get; set; }
        public int ProductId { get; set; }
        public int? RouteId { get; set; }
        public int? Target { get; set; }
        public int? Unit { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? PlanFinish { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualFinish { get; set; }
        public string Status { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
