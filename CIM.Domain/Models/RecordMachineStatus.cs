using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
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
        public int? Hour { get; set; }
        public int? WeekNumber { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndAt { get; set; }

        public virtual Machine Machine { get; set; }
        public virtual MachineStatus MachineStatus { get; set; }
        public virtual ICollection<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
    }
}
