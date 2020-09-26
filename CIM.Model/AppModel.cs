using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class AppModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ThUrl { get; set; }
        public List<AppFeatureModel> FeatureList { get; set; } = new List<AppFeatureModel>();
    }
}
