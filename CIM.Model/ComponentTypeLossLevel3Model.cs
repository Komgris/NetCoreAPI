﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class ComponentTypeLossLevel3Model
    {
        public int Id { get; set; }
        public int MachineComponentTypeId { get; set; }
        public int LossLevel3Id { get; set; }
        //public virtual LossLevel3 LossLevel3 { get; set; }
        //public virtual ComponentType MachineComponentType { get; set; }
    }
}
