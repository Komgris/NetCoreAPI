using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MaintenanceActivity
    {
        public int Id { get; set; }
        public int MaintenanceId { get; set; }
        public string Details { get; set; }
        public bool? IsActive { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual MaintenancePlan Maintenance { get; set; }
    }
}
