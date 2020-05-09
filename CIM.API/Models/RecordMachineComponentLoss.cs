using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class RecordMachineComponentLoss
    {
        public int Id { get; set; }
        public int MachineComponentId { get; set; }
        public int LossLevel3Id { get; set; }
        public int? RecordMachineComponentStatusId { get; set; }
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
        public bool? IsAuto { get; set; }
        public string Guid { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual LossLevel3 LossLevel3 { get; set; }
        public virtual Component MachineComponent { get; set; }
        public virtual ProductionPlan ProductionPlan { get; set; }
        public virtual RecordMachineStatus RecordMachineComponentStatus { get; set; }
        public virtual Users UpdatedByNavigation { get; set; }
    }
}
