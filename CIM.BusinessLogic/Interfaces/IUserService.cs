using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IUserService :  IBaseService
    {
        void Register(RegisterUserModel model);
        AuthModel Auth(string username, string password);
        CurrentUserModel GetCurrentUserModel(string token);
        string HashPassword(RegisterUserModel model);
    }
}
