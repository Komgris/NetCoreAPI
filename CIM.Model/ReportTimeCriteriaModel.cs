using System;
using System.Collections.Generic;
using System.Text;
using static CIM.Model.Constans;

namespace CIM.Model
{
    public class ReportTimeCriteriaModel
    {
        public ReportType ReportType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? WeekFrom { get; set; }
        public int? MonthFrom { get; set; }
        public int? YearFrom { get; set; }
        public int? IPDFrom { get; set; }
        public int? WeekTo { get; set; }
        public int? MonthTo { get; set; }
        public int? YearTo { get; set; }
        public int? IPDTo { get; set; }
    }

    public class ReportDateModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
