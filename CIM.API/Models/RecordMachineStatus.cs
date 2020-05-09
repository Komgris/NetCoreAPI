using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class RecordMachineStatus
    {
        public RecordMachineStatus()
        {
            RecordMachineComponentLoss = new HashSet<RecordMachineComponentLoss>();
        }

        public int Id { get; set; }
        public int MachineId { get; set; }
        public int MachineStatusId { get; set; }
        public string ProductionPlanId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual MachineStatus MachineStatus { get; set; }
        public virtual ICollection<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
    }
}
