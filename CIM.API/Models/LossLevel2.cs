using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class LossLevel2
    {
        public LossLevel2()
        {
            LossLevel3 = new HashSet<LossLevel3>();
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
        public int LossLevel1Id { get; set; }

        public virtual LossLevel1 LossLevel1 { get; set; }
        public virtual ICollection<LossLevel3> LossLevel3 { get; set; }
    }
}
