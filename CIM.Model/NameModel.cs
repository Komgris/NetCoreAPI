using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class NameModel
    {
        public int Id { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int? EmployeesId { get; set; }
        public string UserId { get; set; }
    }
}
