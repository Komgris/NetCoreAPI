using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionOutputModel
    {
        public string ProductionPlanId { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime Start { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Amount { get; set; }
        public int AdditionalCounterOut { get; set; }

    }
}
