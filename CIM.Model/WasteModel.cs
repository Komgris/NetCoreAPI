using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class WasteModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int MachineId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

    }
}
