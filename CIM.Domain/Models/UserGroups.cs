﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class UserGroups
    {
        public UserGroups()
        {
            UserGroupLocal = new HashSet<UserGroupLocal>();
            UserGroupsApps = new HashSet<UserGroupsApps>();
            UsersUserGroups = new HashSet<UsersUserGroups>();
        }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public virtual ICollection<UserGroupLocal> UserGroupLocal { get; set; }
        public virtual ICollection<UserGroupsApps> UserGroupsApps { get; set; }
        public virtual ICollection<UsersUserGroups> UsersUserGroups { get; set; }
    }
}
