using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class AppFeatures
    {
        public AppFeatures()
        {
            UserGroupsAppFeatures = new HashSet<UserGroupsAppFeatures>();
        }

        public int FeatureId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int AppId { get; set; }

        public virtual App App { get; set; }
        public virtual ICollection<UserGroupsAppFeatures> UserGroupsAppFeatures { get; set; }
    }
}
