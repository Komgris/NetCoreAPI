using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MaterialGroupMaterial
    {
        public int Id { get; set; }
        public int MaterialGroupId { get; set; }
        public int MaterialId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual Material Material { get; set; }
        public virtual MaterialGroup MaterialGroup { get; set; }
    }
}
