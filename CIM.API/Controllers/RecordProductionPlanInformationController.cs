using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    public class RecordProductionPlanInformationController : BaseController
    {
        private IRecordProductionPlanInformationService _service;
        public RecordProductionPlanInformationController(
            IRecordProductionPlanInformationService service
            )
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ProcessReponseModel<RecordProductionPlanInformationModel>> Create(RecordProductionPlanInformationModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanInformationModel>();
            try
            {
                output.Data = await _service.Compare(model);
                //var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{model.ProductionPlanId}";
                //var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(rediskey);

                //if (productionPlan != null)
                //{
                //    await HandleBoardcastingActiveProcess(DataTypeGroup.Waste, model.ProductionPlanId
                //        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                //    //dole dashboard
                //    _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.Waste);
                //}

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