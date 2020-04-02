using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionPlanListModel
    {
        public string Id { get; set; }
        public string Line { get; set; }

        public string Product { get; set; }

        public int? Target { get; set; }

        public int? Unit { get; set; }

        public string Status { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Finished { get; set; }

        public DateTime? ActualStart { get; set; }

        public DateTime? ActualFinish { get; set; }

    }
}
