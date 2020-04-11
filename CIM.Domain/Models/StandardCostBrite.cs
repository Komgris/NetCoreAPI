using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class StandardCostBrite
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ContainerSize { get; set; }
        public string PackSize { get; set; }
        public int ProductTypeId { get; set; }
        public decimal AllBhtperUnit { get; set; }
        public decimal FruitBhtperUnit { get; set; }
        public decimal PackingMediumBhtperUnit { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ProductType ProductType { get; set; }
    }
}
