using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RouteProductGroupModel
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int ProductGroupId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
