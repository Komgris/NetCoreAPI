using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Users
    {
        public Users()
        {
            Employees = new HashSet<Employees>();
            Name = new HashSet<Name>();
            SitesUsers = new HashSet<SitesUsers>();
            UserProfiles = new HashSet<UserProfiles>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsSuspend { get; set; }
        public int UserGroupId { get; set; }
        public string DefaultLanguageId { get; set; }

        public virtual UserGroups UserGroup { get; set; }
        public virtual UserAppTokens UserAppTokens { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<Name> Name { get; set; }
        public virtual ICollection<SitesUsers> SitesUsers { get; set; }
        public virtual ICollection<UserProfiles> UserProfiles { get; set; }
    }
}
