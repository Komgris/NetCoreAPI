using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MappingMachineTypeComponentTypeModel<T>
        where T : new()
    {
        public T Component { get; set; } = new T();
        public int MachineId { get; set; }
    }
}
