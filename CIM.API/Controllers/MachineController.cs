﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Cache;
using CIM.API.HubConfig;
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
    public class MachineController : ControllerBase
    {
        private IHubContext<MachineHub> _hub;
        private IResponseCacheService _responseCacheService;

        public MachineController(IHubContext<MachineHub> hub,
            IResponseCacheService responseCacheService)
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
        }

        public IActionResult Get()
        {
            var timerManager = new TimerService(() =>  _hub.Clients.All.SendAsync("transfermachinedata", DataService.GetData()));

            return Ok(new { Message = "Request Machine Completed" });
        }

    }
}