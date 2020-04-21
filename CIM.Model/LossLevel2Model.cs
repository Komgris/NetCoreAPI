using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class LossLevel2Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int? LossLevel1Id { get; set; }
    }
}
