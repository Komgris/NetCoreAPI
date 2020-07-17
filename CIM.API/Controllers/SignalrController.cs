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
    public class SignalrController : BoardcastController
    {

        public SignalrController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IReportService service,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
        ) : base(hub, responseCacheService, service, config, activeProductionPlanService)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">transfer-message, command-channel</param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Send(string channel, string data)
        {
            await _hub.Clients.All.SendAsync(channel, data);
            return "OK";
        }


    }
}
