using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductionPlan = new HashSet<ProductionPlan>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string BriteItemPerUPCItem { get; set; }
        public int ProductFamilyId { get; set; }
        public int ProductGroupId { get; set; }
        public int ProductTypeId { get; set; }
        public string PackingMedium { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? IGWeight { get; set; }
        public decimal? PMWeight { get; set; }
        public decimal? WeightPerUOM { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ProductFamily ProductFamily { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<ProductionPlan> ProductionPlan { get; set; }
    }
}
