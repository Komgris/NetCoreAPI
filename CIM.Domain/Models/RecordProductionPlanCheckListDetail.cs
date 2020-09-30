using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordProductionPlanCheckListDetail
    {
        public int Id { get; set; }
        public int RecordCheckListId { get; set; }
        public int CheckListId { get; set; }
        public bool IsCheck { get; set; }
        public string Remark { get; set; }

        public virtual ProductionPlanCheckList CheckList { get; set; }
        public virtual RecordProductionPlanCheckList RecordCheckList { get; set; }
    }
}
