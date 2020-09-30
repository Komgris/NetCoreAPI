using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanCheckListModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? CheckListTypeId { get; set; }
        public int MachinTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Remark { get; set; }
        public int? Sequence { get; set; }
    }
}
