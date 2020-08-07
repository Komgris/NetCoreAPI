using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RecordActiveProductionPlansModel
    {
        public int Id { get; set; }
        public string ProductionPlanPlanId { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Finish { get; set; }
        public DateTime? EstimateFinish { get; set; }
        public int? StatusId { get; set; }
        public int Target { get; set; }
        public string CreatedBy { get; set; }
        public int? OperatorSetId { get; set; }
    }

}
