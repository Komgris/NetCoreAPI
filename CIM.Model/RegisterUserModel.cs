using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class RegisterUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Image { get; set; }
        public int UserGroup_Id { get; set; }
    }
}
