using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CIM.Model
{
    public class MaterialDictionaryModel
    {
        public int Id { get; set; }
        public string MaterialDescription { get; set; }
        //Need to be commented, tp avoid material id duplication
        //public int BOMId { get; set; }
        [JsonIgnore]
        public int ProductId { get; set; }
        public string UOM { get; set; }
    }
}
