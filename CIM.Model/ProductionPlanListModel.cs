using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanListModel
    {
        public string PlanId { get; set; }
        public int? RouteId { get; set; }
        public string Route { get; set; }

        public int ProductId { get; set; }
        public string Product { get; set; }
        public int ProductGroupId { get; set; }
        public string ProductGroup { get; set; }

        public int? Target { get; set; }

        public string Unit { get; set; }

        public string Status { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Finished { get; set; }

        public DateTime? ActualStart { get; set; }

        public DateTime? ActualFinish { get; set; }

    }
}
