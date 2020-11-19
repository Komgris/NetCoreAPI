using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MaterialMasterShowModel
    {
        public string CodeName { get; set; }
        public string Description { get; set; }
        public string MaterialGroup { get; set; }
        public string UnitName { get; set; }
        public double BHTPerUnit { get; set; }
        public string ColorName { get; set; }
    }
}
