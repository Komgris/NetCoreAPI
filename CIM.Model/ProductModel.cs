using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public partial class ProductModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string BriteItemUpcItem { get; set; }
        public int ProductFamilyId { get; set; }
        public int ProductGroupId { get; set; }
        public int ProductTypeId { get; set; }
        public string PackingMedium { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? IgWeight { get; set; }
        public decimal? PmWeight { get; set; }
        public decimal? WeightUom { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
