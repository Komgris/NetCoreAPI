using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Domain.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? ProductCategory { get; set; }
        public string? ICSGroup { get; set; }
        public string? MaterialGroup { get; set; }
        public string? UOM { get; set; }
        public decimal? BHTPerUnit { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? UpdatedBy { get; set; }
    }
}
