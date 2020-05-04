using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class FilterLoadProductionPlanListModel
    {
        public IDictionary<int, string> Products { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> Routes { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> Status { get; set; } = new Dictionary<int, string>();
    }
}
