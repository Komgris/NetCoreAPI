using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductionStatus
    {
        public ProductionStatus()
        {
            ProductionPlan = new HashSet<ProductionPlan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<ProductionPlan> ProductionPlan { get; set; }
    }
}
