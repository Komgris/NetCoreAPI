using System;
using System.Collections.Generic;

namespace CIM.Model
{
    public class RouteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool InProcess { get; set; }
        public int? ParentId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public Dictionary<int,MachineModel> MachineList { get; set; }

    }
}