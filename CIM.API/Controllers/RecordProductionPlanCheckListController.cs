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

namespace CIM.API.Controllers
{
    [ApiController]
   
    public class RecordProductionPlanCheckListController : BaseController
    {
        private IRecordProductionPlanCheckListService _service;
        private IUtilitiesService _utilitiesService;

        public RecordProductionPlanCheckListController(
            IHubContext<GlobalHub> hub,
            IRecordProductionPlanCheckListService service,
            IUtilitiesService utilitiesService,
            IMasterDataService masterDataService
            )
        {
            _hub = hub;
            _service = service;
            _utilitiesService = utilitiesService;
            _masterDataService = masterDataService;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<List<RecordProductionPlanCheckListModel>>> List(string planId)
        {
            var output = new ProcessReponseModel<List<RecordProductionPlanCheckListModel>>();
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

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ProcessReponseModel<RecordProductionPlanCheckListModel>> Create(RecordProductionPlanCheckListModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanCheckListModel>();
            try
            {
                output.Data = await _service.Create(model);
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