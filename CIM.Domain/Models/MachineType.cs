using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool HasOee { get; set; }
        public bool? IsCounterStd { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
