using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MaterialType
    {
        public MaterialType()
        {
            Material = new HashSet<Material>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Material> Material { get; set; }
    }
}
