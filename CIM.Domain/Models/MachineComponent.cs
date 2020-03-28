using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MachineId { get; set; }
        public int TypeId { get; set; }

        public virtual Machine Machine { get; set; }
        public virtual MachineComponentType Type { get; set; }
    }
}
