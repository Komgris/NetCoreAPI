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
        public string Line { get; set; }
        public string Wbrt { get; set; }
        public string ItemBrite { get; set; }
        public string Ingredient { get; set; }
        public string RawMaterial { get; set; }
        public string Brix { get; set; }
        public string Acid { get; set; }
        public string Ph { get; set; }
        public decimal? Weight { get; set; }
        public int? TotalLine { get; set; }
        public string Note { get; set; }
        public ProductModel Product { get; set; }
        public int CompareResult { get; set; }

    }
}