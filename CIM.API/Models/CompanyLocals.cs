using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class CompanyLocals
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageId { get; set; }
        public int FId { get; set; }

        public virtual Companies F { get; set; }
    }
}
