using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class Areas
    {
        public Areas()
        {
            AreaLocals = new HashSet<AreaLocals>();
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int SiteId { get; set; }

        public virtual Sites Site { get; set; }
        public virtual ICollection<AreaLocals> AreaLocals { get; set; }
    }
}
