using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MaintenanceActivity
    {
        public int Id { get; set; }
        public int MaintenanceId { get; set; }
        public string Details { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual MaintenancePlan Maintenance { get; set; }
    }
}
