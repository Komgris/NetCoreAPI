using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProductGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? SizeOz { get; set; }
        public int? NorminalSpeed { get; set; }
        public int? Cup2Case { get; set; }
        public int ProcessTypeId { get; set; }
        public string ProcessTypeName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
