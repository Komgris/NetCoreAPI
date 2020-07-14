using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class AppFeatureModel
    {
        public int FeatureId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int AppId { get; set; }
    }
}
