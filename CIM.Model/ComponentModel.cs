using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ComponentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MachineId { get; set; }
        public int TypeId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
