using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace CIM.API.Controllers
{
    [ApiController]
    public class AppController : BaseController
    {
        private IAppService _service;
        public AppController(
            IAppService service
        ) 
        {
            _service = service;
        }        

        [Route("api/[controller]/Get")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<AppModel>>> Get(int userGroupId)
        {
            var output = new ProcessReponseModel<List<AppModel>>();
            try
            {
                output.Data = await _service.Get(userGroupId);
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