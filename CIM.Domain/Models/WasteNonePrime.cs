using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class WasteNonePrime
    {
        public WasteNonePrime()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public  bool IsActive { get; set; }

    }
}
