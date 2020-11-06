using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Color
    {
        public Color()
        {
            RecordProductionPlanColorOrderDetail = new HashSet<RecordProductionPlanColorOrderDetail>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RecordProductionPlanColorOrderDetail> RecordProductionPlanColorOrderDetail { get; set; }
    }
}
