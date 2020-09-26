using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ClientRoute
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int? RouteId { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual Route Route { get; set; }
    }
}
