using System.Collections.Generic;

namespace CIM.Model
{
    public class DictionaryModel 
    {

        public IDictionary<int, string> Products { get; set; } = new Dictionary<int, string>();
        public IDictionary<string, int> ProductsByCode { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, string> Lines { get; set; } = new Dictionary<string, string>();
        public IDictionary<int, object> ComponentAlerts { get; set; } = new Dictionary<int, object>();
        public IDictionary<int, string> ProductionStatus { get; set; } = new Dictionary<int, string>();
        public IDictionary<string, int> Units { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, int> Routes { get; set; } = new Dictionary<string, int>();
    }
}