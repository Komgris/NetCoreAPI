using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecordProductionPlanOutputController : BoardcastController
    {

        public RecordProductionPlanOutputController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IReportService service,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, service, config, activeProductionPlanService)
        {
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> UpdateMachineProduceCounter([FromBody] List<MachineProduceCounterModel> listData,int? hour)
        {
            var hr = hour ?? DateTime.Now.Hour;
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlans = await _activeProductionPlanService.UpdateMachineOutput(listData, hr);
                foreach (var productionPlan in productionPlans)
                {
                    var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan.ProductionPlanId}";
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
