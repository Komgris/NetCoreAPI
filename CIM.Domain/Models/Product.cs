using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Product
    {
        public Product()
        {
            RecordProductionPlanColorOrder = new HashSet<RecordProductionPlanColorOrder>();
            RecordProductionPlanInformation = new HashSet<RecordProductionPlanInformation>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int? UnitId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<RecordProductionPlanColorOrder> RecordProductionPlanColorOrder { get; set; }
        public virtual ICollection<RecordProductionPlanInformation> RecordProductionPlanInformation { get; set; }
    }
}
