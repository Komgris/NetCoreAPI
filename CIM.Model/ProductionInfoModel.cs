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
        public float Oee { get; set; } = 0;
        public float Performance { get; set; } = 0;
        public float Availability { get; set; } = 0;
        public float Quality { get; set; } = 0;
        public string ProductionPlanId { get; set; }
        public string ProductCode { get; set; }
        public string ShopNo { get; set; }
        public int Sequence { get; set; }
        public int Target { get; set; }
        public void ResetProductInfo(int machineId)
        {
            MachineId = machineId;
            ProductionPlanId = "";
            ProductCode = "";
            ShopNo = "";
            Sequence = 0;
            Target = 0;
        }
    }
}
