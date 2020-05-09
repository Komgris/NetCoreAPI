using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class UserPosition
    {
        public UserPosition()
        {
            Employees = new HashSet<Employees>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
