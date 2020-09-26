using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Education
    {
        public int Id { get; set; }
        public string Educational { get; set; }
        public bool? IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
