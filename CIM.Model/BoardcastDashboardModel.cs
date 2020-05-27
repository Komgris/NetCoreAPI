using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model {
    public class BoardcastDashboardModel {
        public BoardcastDashboardModel()
        {}
        public BoardcastDashboardModel(DashboardTimeFrame type)
        {
            Type = type;
        }
        public DashboardTimeFrame Type { get; set; }
        public List<DashboardModel> Dashboards { get; private set; } = new List<DashboardModel>();
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

        public void SetDashboard(DashboardModel dashboard)
        {
            if (dashboard == null) return;
            Dashboards.Remove(Dashboards.Where(x => x.Name == dashboard.Name).FirstOrDefault());
            Dashboards.Add(dashboard);
        }
    }

    public class DashboardModel {
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public object Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
