using System.Collections.Generic;

namespace CIM.Model
{
    public class RouteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<int,MachineModel> MachineList { get; set; }

    }
}