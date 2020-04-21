using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ActiveComponentModel
    {

        public string ProductionPlanId { get; set; }
        public int MachineId { get; set; }
        public int MachineComponentId { get; set; }

        public int StatusId { get; set; }

    }
}
