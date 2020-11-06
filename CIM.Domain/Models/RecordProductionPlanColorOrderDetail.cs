using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordProductionPlanColorOrderDetail
    {
        public int Id { get; set; }
        public int RecordColorId { get; set; }
        public int ColorId { get; set; }
        public int Sequence { get; set; }
        public string Remark { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Color Color { get; set; }
        public virtual RecordProductionPlanColorOrder RecordColor { get; set; }
    }
}
