﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Machine
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int StatusId { get; set; }
        public int MachineTypeId { get; set; }
        public string StatusTag { get; set; }
        public string SpeedTag { get; set; }
        public string CounterOutTag { get; set; }
        public string CounterResetTag { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
