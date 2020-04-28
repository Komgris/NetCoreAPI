using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordManufacturingLoss
    {
        public RecordManufacturingLoss()
        {
            RecordProductionPlanWaste = new HashSet<RecordProductionPlanWaste>();
        }

        public int Id { get; set; }
        public int? MachineId { get; set; }
        public int? ComponentTypeId { get; set; }
        public int LossLevel3Id { get; set; }
        public string ProductionPlanId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndAt { get; set; }
        public long? Timespan { get; set; }
        public int WeekNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string EndBy { get; set; }
        public bool IsAuto { get; set; }
        public string Guid { get; set; }

        public virtual ComponentType ComponentType { get; set; }
        public virtual Users CreatedByNavigation { get; set; }
        public virtual LossLevel3 LossLevel3 { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual ProductionPlan ProductionPlan { get; set; }
        public virtual Users UpdatedByNavigation { get; set; }
        public virtual ICollection<RecordProductionPlanWaste> RecordProductionPlanWaste { get; set; }
    }
}
