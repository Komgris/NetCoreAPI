using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanDictionaryModel
    {
        public string PlanId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int ProductGroupId { get; set; }
        public int? RouteId { get; set; }

    }
}
