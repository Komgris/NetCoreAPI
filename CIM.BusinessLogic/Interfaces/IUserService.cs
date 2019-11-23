using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IUserService :  IBaseService
    {
        void Register(UserModel model);
        Task<AuthModel> Auth(string username, string password);
        CurrentUserModel GetCurrentUserModel(string token);
        string HashPassword(UserModel model);
        Task<string> CreateToken(string userId);
    }
}
