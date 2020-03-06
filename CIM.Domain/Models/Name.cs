using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Domain.Models
{
    public partial class Name
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Language_id { get; set; }
        public int Employee_Id { get; set; }
        public int User_Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
