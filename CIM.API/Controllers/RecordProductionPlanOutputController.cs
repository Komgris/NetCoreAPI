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
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecordProductionPlanOutputController : BaseController
    {
        private IActiveProductionPlanService _activeProductionPlanService;
        private IRecordProductionPlanOutputService _recordProductionPlanOutputService;

        public RecordProductionPlanOutputController(
            IResponseCacheService responseCacheService,
            IHubContext<GlobalHub> hub,
            IActiveProductionPlanService activeProductionPlanService,
            IRecordProductionPlanOutputService recordProductionPlanOutputService
            )
        {
            _responseCacheService = responseCacheService;
            _hub = hub;
            _activeProductionPlanService = activeProductionPlanService;
            _recordProductionPlanOutputService = recordProductionPlanOutputService;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> UpdateMachineProduceCounter([FromBody] List<MachineProduceCounterModel> listData, int hour)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlans = await _activeProductionPlanService.UpdateMachineOutput(listData, hour);
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
