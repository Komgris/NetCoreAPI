using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class UserPosition
    {
        public UserPosition()
        {
            UserProfiles = new HashSet<UserProfiles>();
        }

        public int Id { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserProfiles> UserProfiles { get; set; }
    }
}
