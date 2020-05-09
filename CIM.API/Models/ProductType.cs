﻿using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            Product = new HashSet<Product>();
            StandardCostBrite = new HashSet<StandardCostBrite>();
            WasteLevel1 = new HashSet<WasteLevel1>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<StandardCostBrite> StandardCostBrite { get; set; }
        public virtual ICollection<WasteLevel1> WasteLevel1 { get; set; }
    }
}
