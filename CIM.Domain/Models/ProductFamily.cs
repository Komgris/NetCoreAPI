﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductFamily
    {
        public ProductFamily()
        {
            Product = new HashSet<Product>();
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
    }
}
