using CIM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IBaseService
    {
        CurrentUserModel CurrentUser { get; }
        JsonSerializerSettings JsonsSetting { get; }
    }
}
