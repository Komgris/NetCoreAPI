using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class TeamEmployeesModel
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int EmployeesId { get; set; }
        public string EmployeesName { get; set; }
        public string EmNo { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Image { get; set; }
    }
}
