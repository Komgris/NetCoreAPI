using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RouteMachineModel
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public int MachineTypeId { get; set; }
        public string MachineTypeName { get; set; }
        public string Image { get; set; }
        public int Sequence { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string ImagePath { get; set; }
    }
}
