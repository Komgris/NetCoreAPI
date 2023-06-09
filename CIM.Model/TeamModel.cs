﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamTypeId { get; set; }
        public string TeamType { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
