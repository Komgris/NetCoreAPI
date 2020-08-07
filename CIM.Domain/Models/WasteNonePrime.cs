using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class WasteNonePrime
    {
        public WasteNonePrime()
        {
            RecordProductionPlanWasteNonePrime = new HashSet<RecordProductionPlanWasteNonePrime>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RecordProductionPlanWasteNonePrime> RecordProductionPlanWasteNonePrime { get; set; }
    }
}
