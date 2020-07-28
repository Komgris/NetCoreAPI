using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class CurrentUserModel
    {
        public bool IsValid { get; set; }
        public string UserId { get; set; }
        public string LanguageId { get; set; }
    }
}
