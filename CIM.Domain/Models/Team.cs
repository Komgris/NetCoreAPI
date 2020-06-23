using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Team
    {
        public Team()
        {
            MachineTeam = new HashSet<MachineTeam>();
            TeamEmployees = new HashSet<TeamEmployees>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamTypeId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual TeamType TeamType { get; set; }
        public virtual ICollection<MachineTeam> MachineTeam { get; set; }
        public virtual ICollection<TeamEmployees> TeamEmployees { get; set; }
    }
}
