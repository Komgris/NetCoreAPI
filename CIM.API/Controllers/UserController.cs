using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic;
using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Implements;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
            
        }

        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<object> Register(RegisterUserModel model)
        {
            try
            {
                var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _service.CurrentUser = currentUser;

                _service.Register(model);
                return new object();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [HttpGet]
        public async Task<AuthModel> Auth(string username, string password)
        {
            try
            {
                var result = await _service.Auth(username, password);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}