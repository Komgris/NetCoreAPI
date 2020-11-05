using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RecordProductionPlanColorOrderDetailModel
    {
        public int Id { get; set; }
        public int RecordColorId { get; set; }
        public int ColorId { get; set; }
        public int Sequence { get; set; }
        public string Remark { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
