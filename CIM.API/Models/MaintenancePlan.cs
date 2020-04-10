using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class MaintenancePlan
    {
        public MaintenancePlan()
        {
            MaintenanceActivity = new HashSet<MaintenanceActivity>();
        }

        public int Id { get; set; }
        public int? MaintenanceTeamId { get; set; }
        public int MachineId { get; set; }
        public int Frequency { get; set; }
        public int Unit { get; set; }
        public int? NotifyDays { get; set; }
        public int? NotifyCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastDate { get; set; }
        public DateTime NextDate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual MaintenanceTeam MaintenanceTeam { get; set; }
        public virtual ICollection<MaintenanceActivity> MaintenanceActivity { get; set; }
    }
}
