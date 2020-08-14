using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachinePanel
    {
        public MachinePanel()
        {
            MachineMonitors = new HashSet<MachineMonitors>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<MachineMonitors> MachineMonitors { get; set; }
    }
}
