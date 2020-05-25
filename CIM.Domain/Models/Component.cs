using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Component
    {
        public Component()
        {
            RecordMachineComponentLoss = new HashSet<RecordMachineComponentLoss>();
            RecordManufacturingLoss = new HashSet<RecordManufacturingLoss>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? MachineId { get; set; }
        public int TypeId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Machine Machine { get; set; }
        public virtual ComponentType Type { get; set; }
        public virtual ICollection<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
        public virtual ICollection<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
    }
}
