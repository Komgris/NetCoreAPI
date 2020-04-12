using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class Employees
    {
        public Employees()
        {
            Name = new HashSet<Name>();
        }

        public int Id { get; set; }
        public string EmNo { get; set; }
        public int Company { get; set; }
        public string EmLevel { get; set; }
        public string UserId { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? TermDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public int UserPositionId { get; set; }
        public string Ddname { get; set; }
        public int? Dddiv { get; set; }
        public int? Dddept { get; set; }
        public string Sex { get; set; }
        public int? Pocod { get; set; }
        public int? Emgrd { get; set; }
        public decimal? Empar { get; set; }
        public string Ddsnam { get; set; }
        public string Emsp2 { get; set; }
        public string Education { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Users User { get; set; }
        public virtual UserPosition UserPosition { get; set; }
        public virtual ICollection<Name> Name { get; set; }
    }
}
