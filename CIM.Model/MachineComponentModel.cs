using System.Collections.Generic;

namespace CIM.Model
{
    public class MachineComponentModel
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public int TypeId { get; set; }

        public int Status { get; set; }
        public int? MachineId { get; set; }

        public int[] LossList { get; set; }
    }
}