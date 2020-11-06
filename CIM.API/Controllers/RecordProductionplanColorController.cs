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
    public class RecordProductionplanColorController : BaseController
    {
        private IRecordProductionPlanColorOrderService _service;
        private IUtilitiesService _utilitiesService;
        public RecordProductionplanColorController(
            IRecordProductionPlanColorOrderService service,
            IUtilitiesService utilitiesService
            )
        {
            _service = service;
            _utilitiesService = utilitiesService;
        }



        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ProcessReponseModel<RecordProductionPlanColorOrderModel>> Create(RecordProductionPlanColorOrderModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanColorOrderModel>();
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

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<List<RecordProductionPlanColorOrderModel>>> List(string planId)
        {
            var output = new ProcessReponseModel<List<RecordProductionPlanColorOrderModel>>();
            try
            {
                output.Data = await _service.List(planId);
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