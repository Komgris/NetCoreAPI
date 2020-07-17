using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    public class UserGroupAppFeatureController : ControllerBase
    {
        private IUserGroupAppFeatureService _service;

        public UserGroupAppFeatureController(IUserGroupAppFeatureService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<List<UserGroupAppFeatureModel>>> Get(int id)
        {
            var output = new ProcessReponseModel<List<UserGroupAppFeatureModel>>();
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