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
    public class MachineComponentController : BaseController
    {

        private IHubContext<MachineHub> _hub;
        private IProductionPlanService _productionPlanService;
        private IResponseCacheService _responseCacheService;
        private IMachineService _machineService;


        public MachineComponentController(
            IHubContext<MachineHub> hub,
            IProductionPlanService productionPlanService,
            IResponseCacheService responseCacheService,
            IMachineService machineService
        )
        {
            _hub = hub;
            _productionPlanService = productionPlanService;
            _responseCacheService = responseCacheService;
            _machineService = machineService;
        }

        [HttpPost]
        public async Task<string> SetStatus(int id, int statusId)
        {

            //get production plan by component

            //update component status in production plan
            var productionPlan = await _productionPlanService.UpdateByComponent(id, statusId);
            //boardcast

            await _hub.Clients.All.SendAsync($"production-plan-{productionPlan.ProductionPlanId}", productionPlan);
            return "OK";
        }

    }
}