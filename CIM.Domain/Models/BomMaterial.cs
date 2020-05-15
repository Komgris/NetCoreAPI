using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class BomMaterial
    {
        public int Id { get; set; }
        public int? BomId { get; set; }
        public int? MaterialId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
