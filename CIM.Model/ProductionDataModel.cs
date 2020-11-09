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
        public List<ProductionUnitDataModel> UnitData { get; private set; } = new List<ProductionUnitDataModel>();
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

        public void SetData(ProductionUnitDataModel dashboard)
        {
            if (dashboard == null) return;
            UnitData.Remove(UnitData.Where(x => x.Name == dashboard.Name).FirstOrDefault());
            UnitData.Add(dashboard);
        }
    }

    public class ProductionUnitDataModel {
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
}
