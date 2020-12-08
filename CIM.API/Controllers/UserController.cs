using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<UserModel>> Create(UserModel model)
        {
            var output = new ProcessReponseModel<UserModel>();
            try
            {
                //await _service.Create(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }


        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<UserModel>>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<UserModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howMany, isActive);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<UserModel>> Get(string id)
        {
            var output = new ProcessReponseModel<UserModel>();
            try
            {
                output.Data = await _service.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPut]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<UserModel>> Update([FromBody] UserModel model)
        {
            var output = new ProcessReponseModel<UserModel>();
            try
            {
                await _service.Update(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetFromUserName")]
        public async Task<ProcessReponseModel<UserModel>> GetFromUserName(string userName)
        {
            var output = new ProcessReponseModel<UserModel>();
            try
            {
                output.Data = await _service.GetFromUserName(userName);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }
    }
}