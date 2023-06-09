﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RecordManufacturingLossModel
    {
        public string ProductionPlanId { get; set; }

        public int MachineId { get; set; }
        public string Machine { get; set; }

        public int? ComponentId { get; set; }

        public int LossLevelId { get; set; }
        public string LossLevel { get; set; }
        public string LossDescription { get; set; }

        public string Guid { get; set; }
        public string ElapseTime { get; set; }
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public bool IsAuto { get; set; }
        public bool IsActive { get; set; }
        public int LossLevel3Id { get; set; }
        public int LossLevel2Id { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int RouteId { get; set; }
        public string Route { get; set; }
        public string Remark { get; set; }
        public bool IsWasteChanged { get; set; }
        public bool IsProcessDriven { get; set; }
        public List<RecordProductionPlanWasteModel> WasteList { get; set; } = new List<RecordProductionPlanWasteModel>();
    }
}
