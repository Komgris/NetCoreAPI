using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class LossLevel3EditableModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? LossLevel2Id { get; set; }
    }
}

