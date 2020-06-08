using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CIM.Model
{
    public partial class ProductModel
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string BriteItemPerUPCItem { get; set; }
        public int ProductFamilyId { get; set; }
        public string ProductFamily { get; set; }
        public int ProductGroupId { get; set; }
        public string ProductGroup { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
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
        public bool IsPlanActive { get; set; }
    }
}
