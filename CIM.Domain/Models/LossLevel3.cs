using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class LossLevel3
    {
        public LossLevel3()
        {
            MachineComponentTypeLossLevel3 = new HashSet<MachineComponentTypeLossLevel3>();
            MachineTypeLossLevel3 = new HashSet<MachineTypeLossLevel3>();
            RecordMachineComponentLoss = new HashSet<RecordMachineComponentLoss>();
            RecordProductionPlanLoss = new HashSet<RecordProductionPlanLoss>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int LossLevel2Id { get; set; }

        public virtual LossLevel2 LossLevel2 { get; set; }
        public virtual ICollection<MachineComponentTypeLossLevel3> MachineComponentTypeLossLevel3 { get; set; }
        public virtual ICollection<MachineTypeLossLevel3> MachineTypeLossLevel3 { get; set; }
        public virtual ICollection<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
        public virtual ICollection<RecordProductionPlanLoss> RecordProductionPlanLoss { get; set; }
    }
}
