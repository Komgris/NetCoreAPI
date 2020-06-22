using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class TeamEmployees
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int EmployeesId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Employees Employees { get; set; }
        public virtual Team Team { get; set; }
    }
}
