using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ActiveRouteModel
    {
        public Dictionary<int, MachineModel> MachineList { get; set; }
        public int? Id { get; set; }
    }
}
