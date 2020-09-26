using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MachineOperatorModel
    {
        public int Id { get; set; }
        public int OperatorCount { get; set; }
        public string PlanId { get; set; }
        public int MachineId { get; set; }

    }
}
