using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class CompaniesSites
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int CompanyId { get; set; }

        public virtual Companies Company { get; set; }
        public virtual Sites Site { get; set; }
    }
}
