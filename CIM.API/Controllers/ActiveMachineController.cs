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
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveMachineController : BaseController {
        private IActiveProductionPlanService _activeProductionPlanService;
        IMasterDataService _masterdataService;

        public ActiveMachineController(
            IResponseCacheService responseCacheService,
            IHubContext<GlobalHub> hub,
            IActiveProductionPlanService activeProductionPlanService,
            IReportService reportService,
            IMasterDataService masterdataService
            )
        {
            _responseCacheService = responseCacheService;
            _hub = hub;
            _activeProductionPlanService = activeProductionPlanService;
            _service = reportService;
            _masterdataService = masterdataService;
        }

        [HttpGet]
        public async Task<string> SetStatus(int id, int statusId, bool isAuto = true)
        {
            var productionPlan = await _activeProductionPlanService.UpdateByMachine(id, statusId, isAuto);

            // Production plan of this component doesn't started yet
            if (productionPlan != null)
            {
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan.ProductionPlanId}";
                await HandleBoardcastingActiveProcess(Constans.BoardcastType.ActiveMachineInfo, productionPlan.ProductionPlanId
                    , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
            }
            return "OK";
        }

        [HttpGet]
        public async Task<string> InitialMachineCache()
        {
            try
            {
                var machineList = _masterdataService.Data.Machines;
                foreach (var mc in machineList)
                    await _activeProductionPlanService.UpdateByMachine(mc.Key, 2, true);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}