using System.Collections.Generic;

namespace CIM.Model
{
    public class DictionaryModel 
    {

        public IDictionary<int, string> Products { get; set; } = new Dictionary<int, string>();
        public IDictionary<string, int> ProductsByCode { get; set; } = new Dictionary<string, int>();
        public IDictionary<int, string> Routes { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, object> ComponentAlerts { get; set; } = new Dictionary<int, object>();
        public IDictionary<int, string> ProductionStatus { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> Units { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> CompareResult { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> WastesLevel2 { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> MachineType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ComponentType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ProductType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ProductGroup { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ProductFamily { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> Machine { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> MaterialType { get; set; } = new Dictionary<int, string>();

    }
}