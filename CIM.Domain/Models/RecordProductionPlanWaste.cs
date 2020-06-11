using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordProductionPlanWaste
    {
        public RecordProductionPlanWaste()
        {
            RecordProductionPlanWasteMaterials = new HashSet<RecordProductionPlanWasteMaterials>();
        }

        public int Id { get; set; }
        public string ProductionPlanId { get; set; }
        public int RouteId { get; set; }
        public int WasteLevel2Id { get; set; }
        public int? CauseMachineId { get; set; }
        public string Reason { get; set; }
        public int? RecordManufacturingLossId { get; set; }
        public int Hour { get; set; }
        public int WeekNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual RecordManufacturingLoss RecordManufacturingLoss { get; set; }
        public virtual WasteLevel2 WasteLevel2 { get; set; }
        public virtual ICollection<RecordProductionPlanWasteMaterials> RecordProductionPlanWasteMaterials { get; set; }
    }
}
