using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IBaseService
    {
        CurrentUserModel CurrentUser { get; set; }
    }
}
