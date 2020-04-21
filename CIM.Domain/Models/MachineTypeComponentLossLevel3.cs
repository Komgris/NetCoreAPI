using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Domain.Models
{
    public partial class MachineTypeComponentLossLevel3
    {
        public int Id { get; set; }
        public int MachineTypeComponentId { get; set; }
        public int LossLevel3Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual LossLevel3 LossLevel3 { get; set; }
    }
}
