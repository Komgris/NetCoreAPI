using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class LossLevel3DictionaryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProcessTypeId { get; set; }
        public int? LossLevel2Id { get; set; }
        public int[] Components { get; set; }
        public bool IsProcessDriven { get; set; }
        public bool IsMP { get; set; }
    }
}
