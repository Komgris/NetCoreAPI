using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineType
    {
        public MachineType()
        {
            Machine = new HashSet<Machine>();
            MachineTypeLossLevel3 = new HashSet<MachineTypeLossLevel3>();
            MachineTypeMaterial = new HashSet<MachineTypeMaterial>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool HasOee { get; set; }
        public bool? IsCounterStd { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Machine> Machine { get; set; }
        public virtual ICollection<MachineTypeLossLevel3> MachineTypeLossLevel3 { get; set; }
        public virtual ICollection<MachineTypeMaterial> MachineTypeMaterial { get; set; }
    }
}
