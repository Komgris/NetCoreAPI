using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IUserGroupService :  IBaseService
    {
        Task Create(UserGroupModel model);
        Task<PagingModel<UserGroupModel>> List(string keyword, int page, int howmany, bool isActive);
        Task<UserGroupModel> Get(int id);
        Task Update(UserGroupModel model);
    }
}
