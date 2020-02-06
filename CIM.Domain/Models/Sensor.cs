using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int MachineId { get; set; }
        public string PlcAddress { get; set; }
        public string Position { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Machine Machine { get; set; }
    }
}
