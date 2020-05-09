using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class MaintenanceTeam
    {
        public MaintenanceTeam()
        {
            MaintenancePlan = new HashSet<MaintenancePlan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<MaintenancePlan> MaintenancePlan { get; set; }
    }
}
