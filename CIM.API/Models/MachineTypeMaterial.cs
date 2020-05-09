using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class MachineTypeMaterial
    {
        public int Id { get; set; }
        public int MachineTypeId { get; set; }
        public int MaterialId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual MachineType MachineType { get; set; }
    }
}
