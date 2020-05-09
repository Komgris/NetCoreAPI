using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class ComponentTypeLossLevel3
    {
        public int Id { get; set; }
        public int ComponentTypeId { get; set; }
        public int LossLevel3Id { get; set; }

        public virtual ComponentType ComponentType { get; set; }
        public virtual LossLevel3 LossLevel3 { get; set; }
    }
}
