using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Companies
    {
        public Companies()
        {
            CompaniesSites = new HashSet<CompaniesSites>();
            CompanyLocals = new HashSet<CompanyLocals>();
        }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<CompaniesSites> CompaniesSites { get; set; }
        public virtual ICollection<CompanyLocals> CompanyLocals { get; set; }
    }
}
