using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MachineTeam
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public int TeamId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Machine Machine { get; set; }
        public virtual Team Team { get; set; }
    }
}
