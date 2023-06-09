﻿using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model
{
    public class ActiveProductionPlanModel
    {
        public ActiveProductionPlanModel(string planId)
        {
            ProductionPlanId = planId;
        }
        public string ProductionPlanId { get; set; }

        /// <summary>
        /// Key = RouteId
        /// </summary>
        public Dictionary<int,ActiveProcessModel> ActiveProcesses { get; set; } = new Dictionary<int, ActiveProcessModel>();

        //public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();
        public PRODUCTION_PLAN_STATUS Status { get; set; }
        public Dictionary<int, ActiveMachineModel> Machines { get; set; }
    }
}
