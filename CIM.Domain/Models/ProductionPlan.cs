﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductionPlan
    {
        public ProductionPlan()
        {
            RecordActiveProductionPlan = new HashSet<RecordActiveProductionPlan>();
            RecordMachineComponentLoss = new HashSet<RecordMachineComponentLoss>();
            RecordManufacturingLoss = new HashSet<RecordManufacturingLoss>();
        }

        public string PlanId { get; set; }
        public int ProductId { get; set; }
        public int? RouteId { get; set; }
        public int Target { get; set; }
        public int? UnitId { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? PlanFinish { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualFinish { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string Line { get; set; }
        public string Wbrt { get; set; }
        public string ItemBrite { get; set; }
        public string Ingredient { get; set; }
        public string RawMaterial { get; set; }
        public string Brix { get; set; }
        public string Acid { get; set; }
        public string Ph { get; set; }
        public string Weight { get; set; }
        public int? TotalLine { get; set; }
        public string Note { get; set; }
        public string Country { get; set; }
        public string Pm { get; set; }

        public virtual Product Product { get; set; }
        public virtual Route Route { get; set; }
        public virtual ProductionStatus Status { get; set; }
        public virtual Units Unit { get; set; }
        public virtual ICollection<RecordActiveProductionPlan> RecordActiveProductionPlan { get; set; }
        public virtual ICollection<RecordMachineComponentLoss> RecordMachineComponentLoss { get; set; }
        public virtual ICollection<RecordManufacturingLoss> RecordManufacturingLoss { get; set; }
    }
}
