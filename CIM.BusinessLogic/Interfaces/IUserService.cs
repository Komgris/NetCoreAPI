using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IUserService :  IBaseService
    {
        Task Create(UserModel model);
        Task<AuthModel> Auth(string username, string password);
        CurrentUserModel GetCurrentUserModel(string token);
        string HashPassword(UserModel model);
        Task<string> CreateToken(string userId);
        Task<PagingModel<UserModel>> List(string keyword, int page, int howmany, bool isActive);
        Task<UserModel> Get(string id);
        Task Update(UserModel model);
        Task<UserModel> GetFromUserName(string userName);
    }
}
