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
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    //SignalR used
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ActiveProcessController : BaseController
    {
        //private IHubContext<MachineHub> _hub;
        private IProductionPlanService _productionPlanService;
        private IActiveProductionPlanService _activeProductionPlanService;

        public ActiveProcessController(IHubContext<MachineHub> hub,
            IProductionPlanService productionPlanService,
            IActiveProductionPlanService activeProductionPlanService
            )
        {
            _hubMachine = hub;
            _productionPlanService = productionPlanService;
            _activeProductionPlanService = activeProductionPlanService;
        }

        public IActionResult Get()
        {
            return Ok(new { Message = "Active Process Channel Open." });
        }

        [Route("ProductionPlan")]
        [HttpGet]
        //Use to open channel
        public async Task<ProcessReponseModel<object>> OpenChannel(string productionPlanId)
        {
            var output = new ProcessReponseModel<object>();

            try
            {
                var productionPlan = await _activeProductionPlanService.GetCached(productionPlanId); 
                output.Data = JsonConvert.SerializeObject(productionPlan, JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;

        }

        [Route("TakeAction")]
        [HttpGet]
        public async Task<ProcessReponseModel<object>> TakeAction(string productionPlanId)
        {
            var output = new ProcessReponseModel<object>();

            try
            {
                var productionPlan = await _productionPlanService.TakeAction(productionPlanId);

                // Production plan of this component doesn't started yet
                if (productionPlan != null)
                {
                    var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlanId}";
                    await _hubMachine.Clients.All.SendAsync(channelKey, JsonConvert.SerializeObject(productionPlan, JsonsSetting));
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