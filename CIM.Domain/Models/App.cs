﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class App
    {
        public App()
        {
            AppFeatures = new HashSet<AppFeatures>();
            UserGroupsApps = new HashSet<UserGroupsApps>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ThUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public virtual ICollection<AppFeatures> AppFeatures { get; set; }
        public virtual ICollection<UserGroupsApps> UserGroupsApps { get; set; }
    }
}
