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
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveMachineController : BoardcastController
    {

        public ActiveMachineController(
            IActiveProductionPlanService activeProductionPlanService,
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IReportService service,
            IConfiguration config
            ) : base(hub, responseCacheService, service, config, activeProductionPlanService)
        {
        }

        [HttpGet]
        public async Task<string> SetStatus(int id, int statusId, bool isAuto = true)
        {
            var productionPlan = await _activeProductionPlanService.UpdateByMachine(id, statusId, isAuto);

            // Production plan of this component doesn't started yet
            if (productionPlan != null)
            {
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan.ProductionPlanId}";
                await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveMachineInfo, productionPlan.ProductionPlanId
                    , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
            }
            return "OK";
        }
    }
}