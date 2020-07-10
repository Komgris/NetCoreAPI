using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ViewProductInfo
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public double? SizeOz { get; set; }
        public int? NorminalSpeed { get; set; }
        public int? Cup2Case { get; set; }
    }
}
