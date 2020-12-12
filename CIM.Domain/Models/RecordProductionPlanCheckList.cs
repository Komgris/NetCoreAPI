using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordProductionPlanCheckList
    {
        public RecordProductionPlanCheckList()
        {
            RecordProductionPlanCheckListDetail = new HashSet<RecordProductionPlanCheckListDetail>();
        }

        public int Id { get; set; }
        public string ProductionPlanId { get; set; }
        public int Hour { get; set; }
        public int Date { get; set; }
        public int WeekNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int? PadNumber { get; set; }

        public virtual ICollection<RecordProductionPlanCheckListDetail> RecordProductionPlanCheckListDetail { get; set; }
    }
}
