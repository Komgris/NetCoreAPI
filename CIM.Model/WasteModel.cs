﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class WasteModel
    {
        public string Description { get; set; }
        public string MachineName { get; set; }
        public int Id { get; set; }
        public int MachineId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public decimal AmountUnit { get; set; }

    }
}
