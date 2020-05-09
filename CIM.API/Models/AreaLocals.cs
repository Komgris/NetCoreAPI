using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class AreaLocals
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageId { get; set; }
        public int FId { get; set; }

        public virtual Areas F { get; set; }
    }
}
