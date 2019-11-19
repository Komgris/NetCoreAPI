using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class UsersUserGroups
    {
        public string UserId { get; set; }
        public int UserGroupId { get; set; }

        public virtual Users User { get; set; }
        public virtual UserGroups UserGroup { get; set; }
    }
}
