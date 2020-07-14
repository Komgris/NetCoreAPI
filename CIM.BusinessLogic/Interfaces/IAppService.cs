using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IAppService : IBaseService
    {
        Task<List<AppModel>> Get(int userGroupId);
    }
}
