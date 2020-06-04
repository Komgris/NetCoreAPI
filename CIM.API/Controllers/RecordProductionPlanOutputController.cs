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
        private IRecordProductionPlanOutputService _recordProductionPlanOutputService;

        public RecordProductionPlanOutputController(
            IResponseCacheService responseCacheService,
            IHubContext<GlobalHub> hub,
            IRecordProductionPlanOutputService recordProductionPlanOutputService
            )
        {
            _responseCacheService = responseCacheService;
            _hub = hub;
            _recordProductionPlanOutputService = recordProductionPlanOutputService;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<bool>> UpdateMachineProduceCounter([FromBody] List<MachineProduceCounterModel> listData)
        {
            var storeData = listData;
            var hour = DateTime.Now.Hour;

            if (storeData != null)
            {
                var Status = await _recordProductionPlanOutputService.UpdateMachineProduceCounter(storeData, hour);
            }
            return new ProcessReponseModel<bool>();

        }
    }
}
