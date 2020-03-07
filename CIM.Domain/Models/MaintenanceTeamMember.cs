using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class MaintenanceTeamMember
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int MaintenanceTeamId { get; set; }
        public bool IsLeader { get; set; }
        public string Response { get; set; }
        public bool IsActive { get; set; }
        public int AddBy { get; set; }
        public DateTime AddDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
