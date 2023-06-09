﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class AuthModel
    {

        public string UserId { get; set; }
        public string FullName { get; set; }
        public bool IsSuccess { get; set; }
        public string Group { get; set; }
        public List<AppModel> Apps { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public int UserGroupId { get; set; }
    }
}
