using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class SitesUsers
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string UserId { get; set; }

        public virtual Sites Site { get; set; }
        public virtual Users User { get; set; }
    }
}
