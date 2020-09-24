using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model {
    public class SystemParameterModel {
        public int Id { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
