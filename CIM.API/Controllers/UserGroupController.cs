using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private IUserGroupService _service;

        public UserGroupController(IUserGroupService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<UserGroupModel>> Create(UserGroupModel model)
        {
            var output = new ProcessReponseModel<UserGroupModel>();
            try
            {
                await _service.Create(model);
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
        public async Task<PagingModel<UserGroupModel>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true)
        {
            try
            {
                var result = await _service.List(keyword, page, howMany, isActive);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<UserGroupModel>> Get(int id)
        {
            var output = new ProcessReponseModel<UserGroupModel>();
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
        public async Task<ProcessReponseModel<UserGroupModel>> Update([FromBody] UserGroupModel model)
        {
            var output = new ProcessReponseModel<UserGroupModel>();
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

    }
}