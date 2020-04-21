﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class RecordProductionOutput
    {
        public int Id { get; set; }
        public string ProductionPlanId { get; set; }
        public int Count { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual ProductionPlan ProductionPlan { get; set; }
        public virtual Users UpdatedByNavigation { get; set; }
    }
}