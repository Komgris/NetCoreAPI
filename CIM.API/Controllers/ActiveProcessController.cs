using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Cache;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    //SignalR used
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ActiveProcessController : BaseController
    {
        private IHubContext<MachineHub> _hub;
        private IResponseCacheService _responseCacheService;
        private IMachineService _machineService;
        private IProductionPlanService _productionPlanService;

        public ActiveProcessController(IHubContext<MachineHub> hub,
            IResponseCacheService responseCacheService,
            IMachineService machineService,
            IProductionPlanService productionPlanService
            )
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
            _machineService = machineService;
            _productionPlanService = productionPlanService;

        }

        public IActionResult Get()
        {
            return Ok(new { Message = "Active Process Channel Open." });
        }

        [Route("/{productionPlanId}")]
        [HttpGet]
        //Use to open channel
        public async Task<ActiveProcessModel> OpenChannel(string productionPlanId)
        {
            return await _responseCacheService.GetAsTypeAsync<ActiveProcessModel>($"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlanId}");
        }

        [Route("TakeAction")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> TakeAction(int productionPlanId)
        {
            var output = new ProcessReponseModel<object>();

            try
            {
                var productionPlan = await _productionPlanService.TakeAction(productionPlanId);

                // Production plan of this component doesn't started yet
                if (productionPlan != null)
                {
                    var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlanId}";
                    await _hub.Clients.All.SendAsync(channelKey, JsonConvert.SerializeObject(productionPlan, JsonsSetting));
                }

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