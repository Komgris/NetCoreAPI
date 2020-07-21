using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ViewRouteMachine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public int Sequence { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool? IsPointOfCounter { get; set; }
        public int MachineTypeId { get; set; }
        public string MachineTypeName { get; set; }
        public bool? IsCounterStd { get; set; }
        public string Image { get; set; }
    }
}
