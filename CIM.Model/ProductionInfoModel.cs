using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionInfoModel
    {
        public ProductionInfoModel() { }

        public float OEE { get; set; } = 0;
        public float Performance { get; set; } = 0;
        public float Availability { get; set; } = 0;
        public float Quality { get; set; } = 0;

        public Dictionary<int, MachineInfoModel> MachineInfoList { get; set; } = new Dictionary<int, MachineInfoModel>();
    }

    public class MachineInfoModel {

        public MachineInfoModel() { }

        public MachineInfoModel(int mcId) : this()
        {
            MachineId = mcId;
        }

        public int MachineId { get; set; }
        public float OEE { get; set; } = 0;
        public float Performance { get; set; } = 0;
        public float Availability { get; set; } = 0;
        public float Quality { get; set; } = 0;

        public string ProductSKU { get; set; }
        public string Description { get; set; }
        public string ShopNo { get; set; }
        public int Target { get; set; }
        public int Defect { get; set; }
        public void ResetMachineInfo(int machineId)
        {
            MachineId = machineId;
            ProductSKU = "";
            Description = "";
            ShopNo = "";
            Target = 0;
            Defect = 0;

            OEE = 0;
            Performance = 0;
            Availability = 0;
            Quality = 0;
        }
    }
}
