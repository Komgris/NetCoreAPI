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

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ActiveProcessController : ControllerBase
    {
        private IHubContext<MachineHub> _hub;
        private IResponseCacheService _responseCacheService;
        private IMachineService _machineService;

        public ActiveProcessController(IHubContext<MachineHub> hub,
            IResponseCacheService responseCacheService,
            IMachineService machineService
            )
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
            _machineService = machineService;
        }

        public IActionResult Get()
        {
            return Ok(new { Message = "Active Process Channel Open." });
        }

    }
}