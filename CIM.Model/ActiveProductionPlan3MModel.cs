﻿using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model
{
    public class ActiveProductionPlan3MModel
    {
        public ActiveProductionPlan3MModel(string planId)
        {
            ProductionPlanId = planId;
        }
        public string ProductionPlanId { get; set; }

        /// <summary>
        /// Key = RouteId
        /// </summary>
        public Dictionary<int, ActiveProcess3MModel> ActiveProcesses { get; set; } = new Dictionary<int, ActiveProcess3MModel>();

        //public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();
        public PRODUCTION_PLAN_STATUS Status { get; set; }
        public Dictionary<int, ActiveMachine3MModel> Machines { get; set; }
    }
}