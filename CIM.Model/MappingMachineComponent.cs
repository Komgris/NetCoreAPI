using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MappingMachineComponent<ComponentModel>
    {
        public ComponentModel ComponentList { get; set; }
        public int MachineId { get; set; }
    }
}
