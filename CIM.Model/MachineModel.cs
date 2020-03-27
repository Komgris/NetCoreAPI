using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MachineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int MachineTypeId { get; set; }
        public string Type { get; set; }
        public string Plcaddress { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
