using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ProductMatMaster
    {
        public string Product { get; set; }
        public string ProductDescription { get; set; }
        public string Iums { get; set; }
        public string BomChild { get; set; }
        public string BomChildDescription { get; set; }
        public string Iums01 { get; set; }
        public string Iump { get; set; }
        public double? Bqreq { get; set; }
        public double? Bmscp { get; set; }
        public string BomChildMaterial { get; set; }
        public string ColorLayer1 { get; set; }
        public string ColorLayer2 { get; set; }
        public string ColorLayer3 { get; set; }
        public string ColorLayer4 { get; set; }
        public string ColorLayer5 { get; set; }
        public string ColorLayer6 { get; set; }
        public double? WorkcenterNo { get; set; }
        public string WorkcenterDescription { get; set; }
    }
}
