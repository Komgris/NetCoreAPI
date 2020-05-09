using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class Sites
    {
        public Sites()
        {
            Areas = new HashSet<Areas>();
            CompaniesSites = new HashSet<CompaniesSites>();
            SitesUsers = new HashSet<SitesUsers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Areas> Areas { get; set; }
        public virtual ICollection<CompaniesSites> CompaniesSites { get; set; }
        public virtual ICollection<SitesUsers> SitesUsers { get; set; }
    }
}
