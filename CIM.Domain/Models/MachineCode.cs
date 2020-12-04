using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int MachineId { get; set; }
        public int? StatusId { get; set; }
    }
}
