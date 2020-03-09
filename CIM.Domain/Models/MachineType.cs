using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineType
    {
        public MachineType()
        {
            LossLevel3 = new HashSet<LossLevel3>();
            Machine = new HashSet<Machine>();
            MachineTypeMaterial = new HashSet<MachineTypeMaterial>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<LossLevel3> LossLevel3 { get; set; }
        public virtual ICollection<Machine> Machine { get; set; }
        public virtual ICollection<MachineTypeMaterial> MachineTypeMaterial { get; set; }
    }
}
