using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IUserService : IBaseService
    {
        Task<string> Create(AdminUsersModel model);
        Task<AuthModel> Auth(string username, string password);
        Task<bool> GetCurrentUserModel(string token, int appId);
        string HashPassword(UserModel model);
        Task<string> CreateToken(string userId);
        Task<PagingModel<UserModel>> List(string keyword, int page, int howmany, bool isActive);
        Task<UserModel> Get(string id);
        Task Update(UserModel model);
        Task<UserModel> GetFromUserName(string userName);
        Task<ProcessReponseModel<object>> VerifyTokenWithApp(string token, int appId);
        Task<ProcessReponseModel<object>> VerifyToken(string token);
        Task Logout(string token);
    }
}
