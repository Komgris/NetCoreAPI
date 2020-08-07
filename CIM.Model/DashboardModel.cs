using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model
{
    public class DashboardModel
    {
        public DashboardModel()
        { }
        public DashboardModel(DataFrame timeFrame)
        {
            Type = timeFrame;
        }
        public DataFrame Type { get; set; } = DataFrame.Default;
        public List<DashboardDataModel> Data { get; private set; } = new List<DashboardDataModel>();
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
    }

    public class DashboardDataModel
    {
        public string Name { get; set; }
        public string JsonData { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
    }
}

