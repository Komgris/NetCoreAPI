using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Name
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int? EmployeeId { get; set; }
        public string UserId { get; set; }

        public virtual Employees Employee { get; set; }
        public virtual Users User { get; set; }
    }
}
