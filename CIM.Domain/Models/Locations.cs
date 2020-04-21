using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Locations
    {
        public Locations()
        {
            InverseParent = new HashSet<Locations>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Locations Parent { get; set; }
        public virtual ICollection<Locations> InverseParent { get; set; }
    }
}
