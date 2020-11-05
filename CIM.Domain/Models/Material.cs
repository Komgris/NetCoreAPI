using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Material
    {
        public Material()
        {
            RecordProductionPlanInformationDetail = new HashSet<RecordProductionPlanInformationDetail>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string MaterialGroup { get; set; }
        public int? UnitsId { get; set; }
        public int? ColorId { get; set; }
        public decimal? BhtperUnit { get; set; }
        public int? MaterialTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RecordProductionPlanInformationDetail> RecordProductionPlanInformationDetail { get; set; }
    }
}
