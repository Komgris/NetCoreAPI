using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class UserGroupsAppFeatures
    {
        public int FeatureId { get; set; }
        public int AppUserGroupId { get; set; }

        public virtual AppFeatures Feature { get; set; }
    }
}
