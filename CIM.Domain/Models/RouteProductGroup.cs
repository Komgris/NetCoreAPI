using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RouteProductGroup
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int ProductGroupId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ProductGroup ProductGroup { get; set; }
        public virtual Route Route { get; set; }
    }
}
