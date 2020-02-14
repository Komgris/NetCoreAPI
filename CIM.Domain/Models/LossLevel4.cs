﻿using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class LossLevel4
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? MachineTypeId { get; set; }
        public string SymtomId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
