using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class BomMaterialModel
    {
        public int Id { get; set; }
        public int BomId { get; set; }
        public string BomName { get; set; }
        public int MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
