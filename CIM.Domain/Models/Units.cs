using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Units
    {
        public int Id { get; set; }
        public string Uom { get; set; }
        public string Name { get; set; }
    }
}
