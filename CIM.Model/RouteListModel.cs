﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RouteListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPlanActive { get; set; }
        public int? ParentId { get; set; }
        public int ProcessTypeId { get; set; }
        public string ProcessType { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

    }
}
