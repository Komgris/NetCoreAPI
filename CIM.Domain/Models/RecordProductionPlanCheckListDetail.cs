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
        public int? ExampleNumber { get; set; }
        public int? CheckListTypeId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ProductionPlanCheckList CheckList { get; set; }
        public virtual CheckListType CheckListType { get; set; }
        public virtual RecordProductionPlanCheckList RecordCheckList { get; set; }
    }
}
