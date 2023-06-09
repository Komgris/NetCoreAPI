﻿using System.Collections.Generic;

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
        public IDictionary<int, string> WastesLevel1 { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> WastesLevel2 { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> MachineType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ComponentType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ProductType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ProductGroup { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ProductFamily { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> Machine { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> MaterialType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> TeamType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> Team { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> UserPosition { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> Education { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> UserGroup { get; set; } = new Dictionary<int, string>();
        public IDictionary<string, string> Language { get; set; } = new Dictionary<string, string>();
        public IDictionary<int, string> App { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> WasteNonePrime { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> ProcessType { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> AccidentCategory { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, string> MachineById { get; set; } = new Dictionary<int, string>();
        public IDictionary<string, int> MachineByCode { get; set; } = new Dictionary<string, int>();
        public IDictionary<string, int> RouteByName { get; set; } = new Dictionary<string, int>();
        public IDictionary<int, ProductDictionaryModel> ProductProcesstype { get; set; } = new Dictionary<int, ProductDictionaryModel>();
        public IDictionary<int, AppModel> LanguageVersion { get; set; } = new Dictionary<int, AppModel>();
        public IDictionary<int, string> Color { get; set; } = new Dictionary<int, string>();
        public IDictionary<int, List<WasteModel>> WasteByMachine { get; set; } = new Dictionary<int, List<WasteModel>>();
    }
}