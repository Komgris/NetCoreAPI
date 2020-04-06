using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorActiveProcessController : BaseController
    {

        private IResponseCacheService _responseCacheService;

        public OperatorActiveProcessController(
            IResponseCacheService responseCacheService
        )
        {
            _responseCacheService = responseCacheService;
        }

        [HttpGet]
        //Use to open channel
        public async Task<ProcessReponseModel<object>> Get(string productionPlanId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var activePrcess = await _responseCacheService.GetAsTypeAsync<ActiveProcessModel>($"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlanId}"); ;
                output.Data = JsonConvert.SerializeObject( activePrcess, JsonsSetting);
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