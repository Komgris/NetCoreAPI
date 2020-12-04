using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionInfoModel
    {
        public ProductionInfoModel() { }

        public ProductionInfoModel(int mcId) : this()
        {
            MachineId = mcId;
        }
        public int MachineId { get; set; }
        public float Intouch_OEE { get; set; } = 0;
        public float Intouch_Performance { get; set; } = 0;
        public float Intouch_Availability { get; set; } = 0;
        public float Intouch_Quality { get; set; } = 0;
        public string PlanId { get; set; }
    }
}
