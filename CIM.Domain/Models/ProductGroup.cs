﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductGroup
    {
        public ProductGroup()
        {
            Product = new HashSet<Product>();
            RouteProductGroup = new HashSet<RouteProductGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double? SizeOz { get; set; }
        public int? NorminalSpeed { get; set; }
        public int? Cup2Case { get; set; }
        public int ProcessTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<RouteProductGroup> RouteProductGroup { get; set; }
    }
}
