using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public partial class MachineTypeLossLevel3Model
    {
        public int Id { get; set; }
        public int MachineTypeId { get; set; }
        public int LossLevel3Id { get; set; }
        public virtual LossLevel3Model LossLevel3 { get; set; }
    }
}
