using System.Collections.Generic;

namespace CIM.Model
{
    public class MachineModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<MachineComponentModel> ComponentList { get; set; }
    }
}