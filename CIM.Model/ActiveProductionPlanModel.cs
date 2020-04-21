using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ActiveProductionPlanModel
    {
        public string ProductionPlanId { get; set; }

        /// <summary>
        /// Key = RouteId
        /// </summary>
        public Dictionary<int,ActiveProcessModel> ActiveProcesses { get; set; } = new Dictionary<int, ActiveProcessModel>();

        public List<AlertModel> Alerts { get; set; } = new List<AlertModel>();
    }
}
