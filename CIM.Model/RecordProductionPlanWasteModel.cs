using System;
using System.Collections.Generic;

namespace CIM.Model
{
    public class RecordProductionPlanWasteModel
    {

        public int Id { get; set; }
        public string ProductionPlanId { get; set; }
        public int RouteId { get; set; }
        public int WasteLevel2Id { get; set; }
        public int? CauseMachineId { get; set; }
        public string Reason { get; set; }
        public int? RecordManufacturingLossId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
        public int WeekNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public List<RecordProductionPlanWasteMaterialModel> Materials { get; set; }

    }
}