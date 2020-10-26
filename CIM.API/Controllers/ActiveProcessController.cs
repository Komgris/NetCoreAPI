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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    //SignalR used
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ActiveProcessController : BoardcastController
    {
        private IProductionPlanService _productionPlanService;

        public ActiveProcessController(IHubContext<GlobalHub> hub,
            IProductionPlanService productionPlanService,
            IResponseCacheService responseCacheService,
            IDashboardService dashboardService,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
            _productionPlanService = productionPlanService;
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
                if (productionPlan == null)
                {
                    productionPlan = new ActiveProductionPlanModel(productionPlanId);
                }

                foreach (var item in productionPlan.ActiveProcesses)
                {
                    item.Value.Alerts = LimitAlert(item.Value.Alerts);
                }

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
        public async Task<ProcessReponseModel<object>> TakeAction(string productionPlanId, int routeId)
        {
            var output = new ProcessReponseModel<object>();

            try
            {
                var productionPlan = await _productionPlanService.TakeAction3M(productionPlanId, routeId);

                // Production plan of this component doesn't started yet
                if (productionPlan != null)
                {
                    var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlanId}";
                    await HandleBoardcastingActiveProcess3M(DataTypeGroup.Machine, productionPlan.ProductionPlanId
                    , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
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