using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class MachineTypeLossLevel3
    {
        public int Id { get; set; }
        public int LossLevel3Id { get; set; }
        public int MachineTypeId { get; set; }

        public virtual LossLevel3 LossLevel3 { get; set; }
        public virtual MachineType MachineType { get; set; }
    }
}
