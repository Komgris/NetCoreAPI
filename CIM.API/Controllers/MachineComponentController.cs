using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
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

        [Route("TakeAction")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> TakeAction(int productionPlanId)
        {
            var output = new ProcessReponseModel<object>();
             
            try
            {
                var productionPlan = await _productionPlanService.TakeAction(productionPlanId);

                // Production plan of this component doesn't started yet
                if (productionPlan != null)
                {
                    var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlanId}";
                    await _hub.Clients.All.SendAsync(channelKey, JsonConvert.SerializeObject(productionPlan, JsonsSetting));
                }

                output.IsSuccess = true;
            } 
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }


    }
}