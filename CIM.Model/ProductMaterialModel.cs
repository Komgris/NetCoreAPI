using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductMaterialModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal IngredientPerUnit { get; set; }
        public int MaterialTypeId { get; set; }
        public string MaterialTypeName { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int UnitsId { get; set; }
        public string UnitsName { get; set; }
        public string LotNo { get; set; }
        public int MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public decimal QuantityUsage { get; set; }
    }
}
