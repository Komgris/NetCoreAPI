using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class UserProfiles
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Image { get; set; }
        public string UserId { get; set; }
        public int UserPositionId { get; set; }

        public virtual UserPosition UserPosition { get; set; }
    }
}
