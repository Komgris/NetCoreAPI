using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordProductionPlanLossController : BaseController
    {
        private IRecordProductionPlanLossService _recordProductionPlanLossService;
        private IHubContext<MachineHub> _hubContext;
        private IResponseCacheService _responseCacheService;

        public RecordProductionPlanLossController(
            IRecordProductionPlanLossService recordProductionPlanLossService,
            IHubContext<MachineHub> hubContext,
            IResponseCacheService responseCacheService
            )
        {
            _recordProductionPlanLossService = recordProductionPlanLossService;
            _hubContext = hubContext;
            _responseCacheService = responseCacheService;
        }

        [HttpPost]
        public async Task Create(string productionPlanId, int? machibeId, int statusId, bool isAuto = false)
        {
            try
            {

            } 
            catch (Exception ex)
            {

            }
        }


    }
}