using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ManufacturingPerformanceNoMachineModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IDictionary<int, string> LossLevel3 { get; set; }
    }
}
