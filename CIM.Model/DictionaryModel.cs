using System.Collections.Generic;

namespace CIM.Model
{
    public class DictionaryModel 
    {

        public IDictionary<string, string> Products { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> Lines { get; set; } = new Dictionary<string, string>();
        public IDictionary<int, object> ComponentAlerts { get; set; } = new Dictionary<int, object>();
        public IDictionary<string, int> ProductionStatuses { get; set; } = new Dictionary<string, int>();
    }
}