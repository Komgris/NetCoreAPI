using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductionStatusModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool? IsActive { get; set; }
    }
}