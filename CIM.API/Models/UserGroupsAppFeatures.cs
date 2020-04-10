using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class UserGroupsAppFeatures
    {
        public UserGroupsAppFeatures()
        {
            UserGroupsApps = new HashSet<UserGroupsApps>();
        }

        public int FeatureId { get; set; }
        public int AppUserGroupId { get; set; }

        public virtual AppFeatures Feature { get; set; }
        public virtual ICollection<UserGroupsApps> UserGroupsApps { get; set; }
    }
}
