using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class MachineTypeComponentType
    {
        public int Id { get; set; }
        public int ComponentTypeId { get; set; }
        public int MachineTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
