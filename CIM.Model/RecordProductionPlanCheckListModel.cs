using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RecordProductionPlanCheckListModel
    {
        public int Id { get; set; }
        public string ProductionPlanId { get; set; }
        public int CheckListId { get; set; }
        public bool IsChecked { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Remark { get; set; }
    }
}
