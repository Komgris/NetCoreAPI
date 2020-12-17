﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionMasterShowModel
    {
        public string PlanId { get; set; }
        public string ProductCode { get; set; }
        public int MachineType_Id { get; set; }
        public string MachineName { get; set; }
        public string ShopNo { get; set; }
        public decimal? Sequence { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualFinish { get; set; }
        public string StatusName { get; set; }
    }
}