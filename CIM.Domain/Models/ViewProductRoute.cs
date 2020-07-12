using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ViewProductRoute
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int ProductGroupId { get; set; }
        public bool IsActive { get; set; }
        public string GroupName { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
    }
}
