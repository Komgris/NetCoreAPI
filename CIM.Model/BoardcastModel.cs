using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model {
    public class BoardcastModel {
        public BoardcastModel()
        {}
        public BoardcastModel(DashboardDataFrame type)
        {
            Type = type;
        }
        public DashboardDataFrame Type { get; set; }
        public List<BoardcastDataModel> Dashboards { get; private set; } = new List<BoardcastDataModel>();
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

        public void SetDashboard(BoardcastDataModel dashboard)
        {
            if (dashboard == null) return;
            Dashboards.Remove(Dashboards.Where(x => x.Name == dashboard.Name).FirstOrDefault());
            Dashboards.Add(dashboard);
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
