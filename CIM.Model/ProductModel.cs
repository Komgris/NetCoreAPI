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
        public string Description { get; set; }
        public string BriteItemPerUpcitem { get; set; }
        public int ProductFamily_Id { get; set; }
        public int ProductGroup_Id { get; set; }
        public int ProductType_Id { get; set; }
        public string PackingMedium { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? Igweight { get; set; }
        public decimal? Pmweight { get; set; }
        public decimal? WeightPerUom { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
