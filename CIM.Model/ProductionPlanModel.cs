using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanModel
    {
        public string PlanId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int ProductGroupId { get; set; }
        public string ProductGroup { get; set; }
        public int? RouteId { get; set; }
        public string Route { get; set; }
        public int Target { get; set; }
        public int? Unit { get; set; }
        public string UnitName { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? PlanFinish { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualFinish { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public ProductModel Product { get; set; }
        public int CompareResult { get; set; }

    }
}