using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChartController : BoardcastController
    {
        public ChartController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IDashboardService dashboardService,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
        }

        public IActionResult Get()
        {
            var timerManager = new TimerService(() => _hub.Clients.All.SendAsync("transferchartdata", DataService.GetData()));
            return Ok(new { Message = "Request Completed" });
        }
    }
}