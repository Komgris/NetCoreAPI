using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model
{
    public class ActiveProcessModel
    {
        public string ProductionPlanId { get; set; }
        public int ProductId { get; set; }
        public PRODUCTION_PLAN_STATUS Status { get; set; }
        public ActiveRouteModel Route { get; set; }
        public BoardcastModel BoardcastData { get; set; }
    }
}
