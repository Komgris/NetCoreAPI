using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    public class UserGroupAppController : ControllerBase
    {
        private IUserGroupAppService _service;

        public UserGroupAppController(IUserGroupAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<List<UserGroupAppModel>>> Get(int id)
        {
            var output = new ProcessReponseModel<List<UserGroupAppModel>>();
            try
            {
                output.Data = await _service.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }
    }
}