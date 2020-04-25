using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class WasteDictionaryModel
    {

        public int Id { get; set; }

        public string Description { get; set; }

        public int? ProductTypeId { get; set; }

        public int Level { get; set; }
        public int ParentId { get; set; }

        public Dictionary<int, WasteDictionaryModel> Sub { get; set; } = new Dictionary<int, WasteDictionaryModel>();

    }
}
