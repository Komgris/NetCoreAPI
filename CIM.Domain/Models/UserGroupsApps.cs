using System;
using System.Collections.Generic;

namespace CIM.Domain.Models {
    public partial class UserGroupsApps {
        public UserGroupsApps()
        {
            UserGroupsAppFeatures = new HashSet<UserGroupsAppFeatures>();
        }

        public int Id { get; set; }
        public int AppId { get; set; }
        public int UserGroupId { get; set; }
        public virtual App App { get; set; }
        public virtual UserGroups UserGroup { get; set; }
        public virtual ICollection<UserGroupsAppFeatures> UserGroupsAppFeatures { get; set; }
    }
}
