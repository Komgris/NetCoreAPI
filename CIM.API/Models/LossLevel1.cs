using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class LossLevel1
    {
        public LossLevel1()
        {
            LossLevel2 = new HashSet<LossLevel2>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<LossLevel2> LossLevel2 { get; set; }
    }
}
