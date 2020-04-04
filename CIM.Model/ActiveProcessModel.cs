using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ActiveProcessModel
    {
        public string ProductionPlanId { get; set; }
        public int ProductId { get; set; }

        public ActiveRouteModel Route { get; set; }

        public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();

    }
}
