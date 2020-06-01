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
        //private IResponseCacheService _responseCacheService;
        private IHubContext<MachineHub> _hub;
        private IActiveProductionPlanService _activeProductionPlanService;

        public ActiveMachineController(
            IResponseCacheService responseCacheService,
            IHubContext<MachineHub> hub,
            IActiveProductionPlanService activeProductionPlanService
            )
        {
            _responseCacheService = responseCacheService;
            _hub = hub;
            _activeProductionPlanService = activeProductionPlanService;
        }

        [HttpGet]
        public async Task<string> SetStatus(int id, int statusId, bool isAuto = true)
        {
            var productionPlan = await _activeProductionPlanService.UpdateByMachine(id, statusId, isAuto);

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