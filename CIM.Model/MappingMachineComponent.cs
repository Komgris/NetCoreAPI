using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MappingMachineComponent<T>
        where T : new()
    {
        public T ComponentList { get; set; } = new T();
        public int MachineId { get; set; }
    }
}
