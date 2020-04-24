using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordProductionPlanWasteMaterials
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public decimal Amount { get; set; }
        public int WasteId { get; set; }

        public virtual RecordProductionPlanWaste Waste { get; set; }
    }
}
