using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class EmployeesModel
    {
        public string Id { get; set; }
        public int Company { get; set; }
        public string Level { get; set; }
        public string FirstNameEng { get; set; }
        public string LastNameEng { get; set; }
        public string FirstNameTha { get; set; }
        public string LastNameTha { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime TermDate { get; set; }
        public DateTime BirthDate { get; set; }
        public int UserPosition_Id { get; set; }
        public string Ddname { get; set; }
        public int Dddiv { get; set; }
        public int Dddept { get; set; }
        public string Sex { get; set; }
        public int Pocod { get; set; }
        public int Emgrd { get; set; }
        public decimal Empar { get; set; }
        public string Ddsnam { get; set; }
        public string Emsp2 { get; set; }
        public string Education { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
