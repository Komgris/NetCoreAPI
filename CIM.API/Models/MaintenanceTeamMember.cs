using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class MaintenanceTeamMember
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int MaintenanceTeamId { get; set; }
        public bool IsLeader { get; set; }
        public string Response { get; set; }
        public bool IsActive { get; set; }
        public string AddBy { get; set; }
        public DateTime AddDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
