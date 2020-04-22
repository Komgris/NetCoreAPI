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
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveMachineController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private IHubContext<MachineHub> _hub;
        private IProductionPlanService _productionPlanService;

        public ActiveMachineController(
            IResponseCacheService responseCacheService,
            IHubContext<MachineHub> hub,
            IProductionPlanService productionPlanService
            )
        {
            _responseCacheService = responseCacheService;
            _hub = hub;
            _productionPlanService = productionPlanService;
        }

        [HttpGet]
        public async Task<string> SetStatus(int machineId, int statusId)
        {
            var productionPlan = await _productionPlanService.UpdateByMachine(machineId, statusId);

            // Production plan of this component doesn't started yet
            if (productionPlan != null)
            {
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan.ProductionPlanId}";
                await _hub.Clients.All.SendAsync(channelKey, JsonConvert.SerializeObject(productionPlan, JsonsSetting));
            }
            return "OK";

        }
    }
}