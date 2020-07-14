using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class UserGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public List<AppModel> AppList { get; set; }
    }
}