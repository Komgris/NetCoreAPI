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
        private IGenericService _genericService;

        public UserController(
            IUserService service,
            IGenericService genericService
            )
        {
            _service = service;
            _genericService = genericService;


        }

        [HttpPost]
        [MiddlewareFilter(typeof(CustomAuthenticationMiddlewarePipeline))]
        public async Task Create(UserModel model)
        {
            try
            {
                var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _service.CurrentUser = currentUser;

                await _service.Create(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet]
        [MiddlewareFilter(typeof(CustomAuthenticationMiddlewarePipeline))]
        public async Task<PagingModel<UserModel>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            try
            {
                var result = await _service.List(keyword, page, howmany);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}