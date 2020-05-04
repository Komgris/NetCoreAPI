using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class FilterLoadProductionPlanListModel
    {
        public Dictionary<int,string> Products { get; set; }
        public Dictionary<int,string> Routes { get; set; }
        public Dictionary<int,string> Status { get; set; }
    }
}
