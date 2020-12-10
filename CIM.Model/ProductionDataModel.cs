using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model {
    public class ProductionDataModel {
        public ProductionDataModel()
        { }
        public ProductionDataModel(DataFrame timeFrame)
        {
            Type = timeFrame;
        }
        public DataFrame Type { get; set; } = DataFrame.Default;
        public List<UnitDataModel> UnitData { get; private set; } = new List<UnitDataModel>();
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

        public void SetData(UnitDataModel dashboard)
        {
            if (dashboard == null) return;
            UnitData.Remove(UnitData.Where(x => x.Name == dashboard.Name).FirstOrDefault());
            UnitData.Add(dashboard);
        }
    }

    public class UnitDataModel {
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public string JsonData { get; set; }
        public string JsonSpecificData { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }

    public class getdataConfig {
        public string Name { get; set; }
        public string StoreName { get; set; }
        public getdataConfig() { }
        public getdataConfig(string name, string storeName)
        {
            Name = name;
            StoreName = storeName;
        }
    }

    public class KPI
    {
        public float OEE { get; set; }
        public float Performance { get; set; }
        public float Availability { get; set; }
        public float Quality { get; set; }
    }
}
