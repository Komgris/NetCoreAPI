using System;
using System.Collections.Generic;

namespace CIM.Model
{
    public class MachineTeamModel
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public int MachineTypeId { get; set; }
        public string MachineType { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}