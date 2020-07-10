using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class EducationModel
    {
        public int Id { get; set; }
        public string Educational { get; set; }
        public bool? IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
