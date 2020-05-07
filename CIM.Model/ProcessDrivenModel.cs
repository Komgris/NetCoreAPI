using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProcessDrivenModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IDictionary<int, string> LossLevel3 { get; set; }
    }
}
