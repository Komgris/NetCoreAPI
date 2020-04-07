using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineStatus
    {
        public MachineStatus()
        {
            Machine = new HashSet<Machine>();
            RecordMachineComponentStatus = new HashSet<RecordMachineComponentStatus>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Machine> Machine { get; set; }
        public virtual ICollection<RecordMachineComponentStatus> RecordMachineComponentStatus { get; set; }
    }
}
