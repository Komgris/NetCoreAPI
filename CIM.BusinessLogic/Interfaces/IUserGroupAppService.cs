using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IUserGroupAppService :  IBaseService
    {
        //Task Create(UserGroupAppModel model);
        //Task<PagingModel<UserGroupAppModel>> List(string keyword, int page, int howmany, bool isActive);
        Task<List<UserGroupAppModel>> Get(int id);
        //Task Update(UserGroupAppModel model);
    }
}
