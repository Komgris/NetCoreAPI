using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Material
    {
        public Material()
        {
            MaterialGroupMaterial = new HashSet<MaterialGroupMaterial>();
            ProductMaterial = new HashSet<ProductMaterial>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string ProductCategory { get; set; }
        public string Icsgroup { get; set; }
        public string MaterialGroup { get; set; }
        public int? UnitsId { get; set; }
        public string Uom { get; set; }
        public decimal? BhtperUnit { get; set; }
        public int MaterialTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual MaterialType MaterialType { get; set; }
        public virtual Units Units { get; set; }
        public virtual ICollection<MaterialGroupMaterial> MaterialGroupMaterial { get; set; }
        public virtual ICollection<ProductMaterial> ProductMaterial { get; set; }
    }
}
