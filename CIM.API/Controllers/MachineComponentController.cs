using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;

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

            var productionPlan = await _productionPlanService.UpdateByComponent(id, statusId);
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{id}";
            await _hub.Clients.All.SendAsync(channelKey, JsonConvert.SerializeObject(productionPlan) );
            return "OK";
        }

    }
}