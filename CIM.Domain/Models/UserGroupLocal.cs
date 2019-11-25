using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class UserGroupLocal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageId { get; set; }
        public int Fkey { get; set; }

        public virtual UserGroups FkeyNavigation { get; set; }
    }
}
