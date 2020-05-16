using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ActiveRouteModel
    {
        public Dictionary<int, ActiveMachineModel> MachineList { get; set; }
        public int Id { get; set; }
    }
}
