using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model {
    public class BoardcastModel {
        public BoardcastModel()
        { }
        public BoardcastModel(DataFrame timeFrame)
        {
            Type = timeFrame;
        }
        public DataFrame Type { get; set; } = DataFrame.Default;
        public List<BoardcastDataModel> Data { get; private set; } = new List<BoardcastDataModel>();
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

        public void SetData(BoardcastDataModel dashboard)
        {
            if (dashboard == null) return;
            Data.Remove(Data.Where(x => x.Name == dashboard.Name).FirstOrDefault());
            Data.Add(dashboard);
        }
    }

    public class BoardcastDataModel {
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public string JsonData { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
