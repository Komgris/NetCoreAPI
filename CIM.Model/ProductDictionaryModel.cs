using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductDictionaryModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int GroupId { get; set; }
        public int TypeId { get; set; }
        public int ProcessTypeId { get; set; }

        public Dictionary<int, MaterialDictionaryModel> Materials { get; set; }
    }
}
