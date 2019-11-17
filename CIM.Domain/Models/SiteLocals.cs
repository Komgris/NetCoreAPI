using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class SiteLocals
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageId { get; set; }
        public int FId { get; set; }

        public virtual Sites F { get; set; }
    }
}
