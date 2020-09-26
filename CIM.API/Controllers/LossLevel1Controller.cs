using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text.Json;
using CIM.API.HubConfig;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace CIM.API.Controllers
{
    [ApiController]
    public class LossLevel1Controller : BaseController
    {

        private ILossLevel1Service _service;
        public LossLevel1Controller(
            ILossLevel1Service service
        )
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<LossLevel1Model>>> List(string keyword = "", int page = 1, int howmany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<LossLevel1Model>>();
            try
            {
                output.Data = await _service.List(keyword, page, howmany, isActive);
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
