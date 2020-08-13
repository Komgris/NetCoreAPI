using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class AccidentCategory
    {
        public AccidentCategory()
        {
            Accidents = new HashSet<Accidents>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<Accidents> Accidents { get; set; }
    }
}
