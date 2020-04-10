using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineComponent
    {
        public MachineComponent()
        {
            RecordMachineComponentLoss = new HashSet<RecordMachineComponentLoss>();
            RecordMachineComponentStatus = new HashSet<RecordMachineComponentStatus>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MachineId { get; set; }
        public int TypeId { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string KepwareTagAddrr { get; set; }

        public virtual Machine Machine { get; set; }
        public virtual MachineComponentType Type { get; set; }
        public virtual ICollection<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
        public virtual ICollection<RecordMachineComponentStatus> RecordMachineComponentStatus { get; set; }
    }
}
