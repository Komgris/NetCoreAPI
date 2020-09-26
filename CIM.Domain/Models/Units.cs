using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Units
    {
        public Units()
        {
            Material = new HashSet<Material>();
            ProductMaterial = new HashSet<ProductMaterial>();
            ProductionPlan = new HashSet<ProductionPlan>();
        }

        public int Id { get; set; }
        public string Uom { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Material> Material { get; set; }
        public virtual ICollection<ProductMaterial> ProductMaterial { get; set; }
        public virtual ICollection<ProductionPlan> ProductionPlan { get; set; }
    }
}
