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
    }
}