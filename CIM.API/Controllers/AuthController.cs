using System;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;


namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<AuthModel> Auth(string username, string password)
        {
            try
            {
                var result = await _userService.Auth(username, password);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("AuthOperation")]

        public async Task<AuthModel> AuthOperation(string username, string password)
        {
            try
            {
                var result = await _userService.AuthOperation(username, password);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("AuthAdmin")]

        public async Task<AuthModel> AuthAdmin(string username, string password)
        {
            try
            {
                var result = await _userService.AuthAdmin(username, password);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("CreateUser")]
        public async Task<ProcessReponseModel<object>> CreateUser(string username, string password, string email, string name)
        {
            try
            {
                var result = new ProcessReponseModel<object>();
                var results = await _userService.Create(new AdminUsersModel
                {
                    UserName = username,
                    Password = password,
                    Email = email,
                    Name = name
                });

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("VerifyTokenWithApp")]
        public async Task<ProcessReponseModel<object>> VerifyTokenWithApp(string token, int appId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output = await _userService.VerifyTokenWithApp(token, appId);
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("VerifyToken")]
        public async Task<ProcessReponseModel<object>> VerifyToken(string token)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output = await _userService.VerifyToken(token);
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("Logout")]
        public async Task Logout(string token)
        {
            try
            {
                await _userService.Logout(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("HashPassword")]
        public string HashPassword(string password)
        {
            return _userService.HashPassword(password);
        }
    }
}