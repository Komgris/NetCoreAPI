using System;
using System.Collections.Generic;

namespace CIM.API.Models
{
    public partial class UserAppTokens
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredAt { get; set; }

        public virtual Users User { get; set; }
    }
}
