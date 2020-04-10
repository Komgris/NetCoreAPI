using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class UserGroupsApps
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int UserGroupId { get; set; }

        public virtual App App { get; set; }
        public virtual UserGroups UserGroup { get; set; }
        public virtual UserGroupsAppFeatures UserGroupsAppFeatures { get; set; }
    }
}
