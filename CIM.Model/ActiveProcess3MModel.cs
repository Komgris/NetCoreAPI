using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model
{
    public class ActiveProcess3MModel
    {
        public string ProductionPlanId { get; set; }
        public int ProductId { get; set; }
        public PRODUCTION_PLAN_STATUS Status { get; set; }
        //public Dictionary<int, ActiveMachine3MModel> MachineList { get; set; }
        public BoardcastModel BoardcastData { get; set; }
        public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();
        public ActiveMachine3MModel Machine { get; set; }
    }
}
