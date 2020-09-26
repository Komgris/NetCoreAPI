using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public byte[] Image { get; set; }
        public int UserGroupId { get; set; }
        public string DefaultLanguageId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeNo { get; set; }
        public string? OldEmployeeNo { get; set; }
        public string Name { get; set; }
        public string UserGroup { get; set; }
        public bool IsSuspend { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
