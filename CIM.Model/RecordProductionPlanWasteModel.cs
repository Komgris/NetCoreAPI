using System;
using System.Collections.Generic;

namespace CIM.Model
{
    public class RecordProductionPlanWasteModel
    {
        public int Id { get; set; }
        public string ProductionPlanId { get; set; }
        public int RouteId { get; set; }
        public string Route { get; set; }
        public int wasteId { get; set; }
        public int WasteLevel1Id { get; set; }
        public int WasteLevel2Id { get; set; }
        public string WasteLevel2 { get; set; }
        public string WasteDescription { get; set; }
        public int? CauseMachineId { get; set; }
        public string Machine { get; set; }
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

        public List<int> IngredientsMaterials { get; set; }
        public decimal AmountUnit { get; set; }
        public int ProductId { get; set; }
        public DateTime StartedAt { get; set; }

    }
}