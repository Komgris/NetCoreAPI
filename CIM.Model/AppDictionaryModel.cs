using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class AppDictionaryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Dictionary<int, string> FeatureDictionary { get; set; }
    }
}
