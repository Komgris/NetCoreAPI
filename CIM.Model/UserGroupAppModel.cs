using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class UserGroupAppModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string AppName { get; set; }
        public int UserGroupId { get; set; }
    }
}