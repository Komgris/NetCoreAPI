using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RecordProductionPlanCheckListDetailModel
    {
        public int Id { get; set; }
        public int RecordCheckListId { get; set; }
        public int CheckListId { get; set; }
        public bool IsCheck { get; set; }
        public string Remark { get; set; }
    }
}
