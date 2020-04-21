using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineComponentTypeLossLevel3
    {
        public int Id { get; set; }
        public int MachineComponentTypeId { get; set; }
        public int LossLevel3Id { get; set; }

        public virtual LossLevel3 LossLevel3 { get; set; }
        public virtual MachineComponentType MachineComponentType { get; set; }
    }
}
