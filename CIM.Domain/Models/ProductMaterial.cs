using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductMaterial
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        public decimal IngredientPerUnit { get; set; }
        public int UnitsId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual Material Material { get; set; }
        public virtual Product Product { get; set; }
        public virtual Units Units { get; set; }
    }
}
