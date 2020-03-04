using System;
using System.Collections.Generic;

namespace CIM.Domain.Models
{
    public partial class UserProfiles
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string UserId { get; set; }

        public virtual Users User { get; set; }
    }
}
