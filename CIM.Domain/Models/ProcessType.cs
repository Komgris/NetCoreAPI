using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProcessType
    {
        public ProcessType()
        {
            WasteLevel1 = new HashSet<WasteLevel1>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<WasteLevel1> WasteLevel1 { get; set; }
    }
}
