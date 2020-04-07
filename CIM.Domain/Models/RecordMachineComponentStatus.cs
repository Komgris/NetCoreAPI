using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordMachineComponentStatus
    {
        //public RecordMachineComponentStatus()
        //{
        //    RecordMachineComponentLoss = new HashSet<RecordMachineComponentLoss>();
        //}

        public int Id { get; set; }
        public int MachineComponentId { get; set; }
        public int MachineStatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual MachineComponent MachineComponent { get; set; }
        public virtual MachineStatus MachineStatus { get; set; }
        public virtual ICollection<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
    }
}
