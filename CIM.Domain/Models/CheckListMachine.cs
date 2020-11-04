using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class CheckListMachine
    {
        public int Id { get; set; }
        public int CheckListId { get; set; }
        public int MachineId { get; set; }
        public int Sequence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
