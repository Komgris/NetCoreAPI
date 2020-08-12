using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BoardcastController {

        public DashboardController(
        IResponseCacheService responseCacheService,
        IHubContext<GlobalHub> hub,
        IReportService service,
        IConfiguration config,
        IActiveProductionPlanService activeProductionPlanService
        ) : base(hub, responseCacheService, service, config, activeProductionPlanService)
        { }





    }
}