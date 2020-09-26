using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Calendar
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? Year { get; set; }
        public string Iyear { get; set; }
        public string Pdwk { get; set; }
        public string Label { get; set; }
        public int? Wk { get; set; }
        public int? Ipd { get; set; }
        public string Period { get; set; }
        public string Day { get; set; }
        public int? JulainDate { get; set; }
        public int? TjulainDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
