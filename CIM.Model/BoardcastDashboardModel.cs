using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model {
    public class BoardcastDashboardModel {
        public DashboardType Type { get; set; }
        public List<DashboardModel> Dashboard { get; set; }
    }

    public class DashboardModel {
        public string Name { get; set; }
        public object Data { get; set; }
    }
}
