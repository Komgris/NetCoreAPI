﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class BomTemp
    {
        public BomTemp()
        {
            BomMaterial = new HashSet<BomMaterial>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<BomMaterial> BomMaterial { get; set; }
    }
}
