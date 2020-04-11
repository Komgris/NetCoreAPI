using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineComponentType
    {
        public MachineComponentType()
        {
            MachineComponent = new HashSet<MachineComponent>();
            MachineComponentTypeLossLevel3 = new HashSet<MachineComponentTypeLossLevel3>();
            RecordManufacturingLoss = new HashSet<RecordManufacturingLoss>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MachineTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual MachineType MachineType { get; set; }
        public virtual ICollection<MachineComponent> MachineComponent { get; set; }
        public virtual ICollection<MachineComponentTypeLossLevel3> MachineComponentTypeLossLevel3 { get; set; }
        public virtual ICollection<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
    }
}
