using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class SystemParameter
    {
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
