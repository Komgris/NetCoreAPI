using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class UserGroupsApps
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int UserGroupId { get; set; }

        public virtual App App { get; set; }
    }
}
