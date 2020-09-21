using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model {
    public class RecordProductionPlanWasteNonePrimeModel {
        public string ProductionPlanId { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string Description { get; set; }
        public int NonePrimeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
