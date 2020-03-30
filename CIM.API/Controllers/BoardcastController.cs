using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Cache;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardcastController : ControllerBase
    {

        private IHubContext<MachineHub> _hub;


        public BoardcastController(
            IHubContext<MachineHub> hub,
            IMachineService machineService
            )
        {
            _hub = hub;
        }

        [HttpPost]
        public async Task<string> Boardcast(string channel, string data)
        {
            await _hub.Clients.All.SendAsync(channel, data);
            return "OK";
        }

    }
}