using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineMonitors
    {
        public int Id { get; set; }
        public int PanelId { get; set; }
        public int MachineId { get; set; }

        public virtual Machine Machine { get; set; }
        public virtual MachinePanel Panel { get; set; }
    }
}
