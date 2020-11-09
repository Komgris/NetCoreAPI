using System;
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
        public int ProductId { get; set; }
        public PRODUCTION_PLAN_STATUS Status { get; set; }
        public ProductionDataModel ProductionData { get; set; }
        public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();

        /// <summary>
        /// Key = MachineId
        /// </summary>
        //public Dictionary<int, ActiveProcess3MModel> ActiveProcesses { get; set; } = new Dictionary<int, ActiveProcess3MModel>();

        //public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();
        //public Dictionary<int, ActiveMachine3MModel> Machines { get; set; }
        //public DateTime CreateTime = DateTime.Now;
        //public DateTime UpdateTime = DateTime.Now;
        //public string LastAction { get; set; }
        //public bool IsFinished { get; set; } = false;
        //public Dictionary<int, ActiveMachine3MModel> Machines { get; set; }

        //public Dictionary<int, ActiveMachine3MModel> MachineList { get; set; }
        //public ActiveMachine3MModel Machine { get; set; }
    }
}
