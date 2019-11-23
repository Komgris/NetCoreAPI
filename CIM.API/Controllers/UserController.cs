using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
            
        }

        [HttpPost]
        [MiddlewareFilter(typeof(CustomAuthenticationMiddlewarePipeline))]
        public async Task<object> Register(UserModel model)
        {
            try
            {
                var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _service.CurrentUser = currentUser;

                await Task.Run( () => {
                    _service.Register(model);
                    });
                return new object();
            }
            catch (Exception e)
            {
                throw e;
            }

        }


        [HttpGet]
        [MiddlewareFilter(typeof(CustomAuthenticationMiddlewarePipeline))]
        public async Task<List<UserModel>> List()
        {
            try
            {
                return new List<UserModel>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}