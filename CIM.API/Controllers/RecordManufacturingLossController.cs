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
    [ApiController]
    public class RecordManufacturingLossController : BaseController
    {
        private IHubContext<MachineHub> _hub;
        private IRecordManufacturingLossService _recordManufacturingLossService;

        public RecordManufacturingLossController(
            IHubContext<MachineHub> hub,
            IRecordManufacturingLossService recordManufacturingLossService
            )
        {
            _hub = hub;
            _recordManufacturingLossService = recordManufacturingLossService;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<object>> Create(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _recordManufacturingLossService.Create(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPut]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<object>> Update(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlan = await _recordManufacturingLossService.Update(model);
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan.ProductionPlanId}";
                await _hub.Clients.All.SendAsync(channelKey, JsonConvert.SerializeObject(productionPlan, JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetByGuid")]
        public async Task<ProcessReponseModel<RecordManufacturingLossModel>> GetByGuid(Guid guid)
        {
            var output = new ProcessReponseModel<RecordManufacturingLossModel>();
            try
            {
                output.Data = await _recordManufacturingLossService.GetByGuid(guid);
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