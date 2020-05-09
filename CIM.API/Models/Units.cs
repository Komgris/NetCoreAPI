using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class Units
    {
        public Units()
        {
            ProductionPlan = new HashSet<ProductionPlan>();
        }

        public int Id { get; set; }
        public string Uom { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProductionPlan> ProductionPlan { get; set; }
    }
}
