using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MaintenanceTeam
    {
        public MaintenanceTeam()
        {
            MaintenancePlan = new HashSet<MaintenancePlan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<MaintenancePlan> MaintenancePlan { get; set; }
    }
}
