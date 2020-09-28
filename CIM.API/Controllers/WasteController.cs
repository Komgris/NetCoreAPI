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
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WasteController : BoardcastController
    {
        private IRecordProductionPlanWasteService _service;
        ITriggerQueueService _triggerService;

        public WasteController(
            ITriggerQueueService triggerService,
        IRecordProductionPlanWasteService service,
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IDashboardService dashboardService,
            IConfiguration config,
            IRecordManufacturingLossService recordManufacturingLossService,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
            _service = service;
            _triggerService = triggerService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<ProcessReponseModel<PagingModel<RecordProductionPlanWasteModel>>> List(string planId, int? routeId = null, string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<RecordProductionPlanWasteModel>>();
            try
            {
                output.Data = await _service.List(planId, routeId, keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Create(RecordProductionPlanWasteModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                output.Data = await _service.Create(model);
                var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{model.ProductionPlanId}";
                var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(rediskey);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Waste, model.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                    //dole dashboard
                    _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.Waste);
                }

                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPut]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Update(RecordProductionPlanWasteModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                await _service.Update(model);
                var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{model.ProductionPlanId}";
                var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(rediskey);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Waste, model.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                    //dole dashboard
                    _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.Waste);
                }

                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpDelete]
        public async Task<ProcessReponseModel<object>> Delete(int id)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var dbModel = await _service.Get(id);
                await _service.Delete(id);
                var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{dbModel.ProductionPlanId}";
                var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(rediskey);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Waste, dbModel.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                    //dole dashboard
                    _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.Waste);
                }
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Get(int id)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                output.Data = await _service.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }


        [HttpGet]
        [Route("NonePrimeOutputList")]
        public async Task<ProcessReponseModel<PagingModel<RecordProductionPlanWasteNonePrimeModel>>> NonePrimeOutputList(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<RecordProductionPlanWasteNonePrimeModel>>();
            try
            {
                output.Data = await _service.NonePrimeOutputList(keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpPost]
        [Route("NonePrimeCreate")]
        public async Task<ProcessReponseModel<object>> NonePrimeCreate(List<RecordProductionPlanWasteNonePrimeModel> models)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.NonePrimeCreate(models);
                //dole dashboard
                _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.Waste);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("RecordNonePrimeList")]
        public async Task<ProcessReponseModel<object>> RecordNonePrimeList(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _service.RecordNonePrimeList(planId, routeId), JsonsFormatting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [Route("ListByMonth")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<RecordProductionPlanWasteModel>>> ListByMonth(int month, int year, string planId, int? routeId = null)
        {
            var output = new ProcessReponseModel<List<RecordProductionPlanWasteModel>>();
            try
            {
                output.Data = await _service.ListByMonth(month, year, planId, routeId);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [Route("ListByDate")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<RecordProductionPlanWasteModel>>> ListByDate(DateTime date, string keyword, int page, int howmany, string planId, int? routeId = null)
        {
            var output = new ProcessReponseModel<PagingModel<RecordProductionPlanWasteModel>>();
            try
            {
                output.Data = await _service.ListByDate(date, keyword, page, howmany, planId, routeId);
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
