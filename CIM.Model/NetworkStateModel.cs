using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model
{
    public class NetworkStatusModel
    {
        public string Ip { get; set; }
        public string Description { get; set; }
        public NetworkState State { get; set; }
        public DateTime Time { get; set; }
    }
}
