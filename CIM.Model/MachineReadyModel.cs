using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MachineReadyModel
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public int MachineId { get; set; }
        public string ProductionPlanId { get; set; }
    }
}
