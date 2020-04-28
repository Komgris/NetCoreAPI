using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanDictionaryModel
    {
        public string PlanId { get; set; }
        public int? RouteId { get; set; }
        public ProductDictionaryModel Product { get; set; }
    }
}
