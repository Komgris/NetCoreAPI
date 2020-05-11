using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MappingMachineComponent
    {
        public List<ComponentModel> ComponentList { get; set; }
        public int MachineId { get; set; }
    }
}
