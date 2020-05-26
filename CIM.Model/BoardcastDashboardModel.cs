using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model {
    public class BoardcastDashboardModel {
        public BoardcastDashboardModel()
        {

        }
        public BoardcastDashboardModel(DashboardType type)
        {
            Type = type;
        }
        public DashboardType Type { get; set; }
        public List<DashboardModel> Dashboard { get; set; } = new List<DashboardModel>();
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }

    public class DashboardModel {
        public string Name { get; set; }
        public object Data { get; set; }
    }
}
