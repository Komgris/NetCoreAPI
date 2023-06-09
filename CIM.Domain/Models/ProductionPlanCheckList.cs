﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductionPlanCheckList
    {
        public ProductionPlanCheckList()
        {
            RecordProductionPlanCheckListDetail = new HashSet<RecordProductionPlanCheckListDetail>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int? CheckListTypeId { get; set; }
        public int? MachinTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Remark { get; set; }
        public int? Sequence { get; set; }
        public decimal? TrimWaste { get; set; }

        public virtual CheckListType CheckListType { get; set; }
        public virtual ICollection<RecordProductionPlanCheckListDetail> RecordProductionPlanCheckListDetail { get; set; }
    }
}
