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

        [HttpGet]
        [Route("List3M")]
        public async Task<ProcessReponseModel<List<RecordProductionPlanWasteModel>>> List3M(string planId, int? machineId = null, string keyword = "")
        {
            var output = new ProcessReponseModel<List<RecordProductionPlanWasteModel>>();
            try
            {
                output.Data = await _service.List3M(planId, machineId, keyword);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("Get3M")]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Get3M(int id)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                output.Data = await _service.Get3M(id);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpPost]
        [Route("Create3M")]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Create3M(RecordProductionPlanWasteModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                output.Data = await _service.Create3M(model);               

                var productionPlan = _responseCacheService.GetActivePlan(model.ProductionPlanId);
                if (productionPlan != null)
                {
                    var activeMachine = _responseCacheService.GetActiveMachine(model.CauseMachineId);
                    activeMachine.CounterDefect += model.AmountUnit;
                    await _responseCacheService.SetActiveMachine(activeMachine);
                    await HandleBoardcastingActiveMachine3M(model.CauseMachineId);

                    var productionInfo = _responseCacheService.GetProductionInfo();
                    if (productionInfo.MachineInfoList.ContainsKey(model.CauseMachineId))
                    {
                        productionInfo.MachineInfoList[model.CauseMachineId].Defect = activeMachine.CounterDefect;
                        await _responseCacheService.SetProductionInfo(productionInfo);
                    }

                    await HandleBoardcastingActiveProcess3M(DataTypeGroup.Waste, model.ProductionPlanId
                        , model.CauseMachineId, productionPlan);

                    //dole dashboard
                    //_triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.Waste);
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
        [Route("Update3M")]
        public async Task<ProcessReponseModel<RecordProductionPlanWasteModel>> Update3M(RecordProductionPlanWasteModel model)
        {
            var output = new ProcessReponseModel<RecordProductionPlanWasteModel>();
            try
            {
                //FernDev!!!
                await _service.Update3M(model);

                var productionPlan = _responseCacheService.GetActivePlan(model.ProductionPlanId);
                if (productionPlan != null)
                {
                    var activeMachine = _responseCacheService.GetActiveMachine(model.CauseMachineId);
                    activeMachine.CounterDefect += model.AmountUnit - model.AmountUnitOld;
                    await _responseCacheService.SetActiveMachine(activeMachine);
                    await HandleBoardcastingActiveMachine3M(model.CauseMachineId);

                    var productionInfo = _responseCacheService.GetProductionInfo();
                    if (productionInfo.MachineInfoList.ContainsKey(model.CauseMachineId))
                    {
                        productionInfo.MachineInfoList[model.CauseMachineId].Defect = activeMachine.CounterDefect;
                        await _responseCacheService.SetProductionInfo(productionInfo);
                    }

                    await HandleBoardcastingActiveProcess3M(DataTypeGroup.Waste, model.ProductionPlanId
                        , model.CauseMachineId, productionPlan);

                    //dole dashboard
                    //_triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.Waste);
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
        [Route("Delete3M")]
        public async Task<ProcessReponseModel<object>> Delete3M(int id)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var dbModel = await _service.Get3M(id);
                await _service.Delete3M(id);

                var productionPlan = _responseCacheService.GetActivePlan(dbModel.ProductionPlanId);
                if (productionPlan != null)
                {
                    var activeMachine = _responseCacheService.GetActiveMachine(dbModel.CauseMachineId);
                    activeMachine.CounterDefect -= dbModel.AmountUnit;
                    await _responseCacheService.SetActiveMachine(activeMachine);
                    await HandleBoardcastingActiveMachine3M(dbModel.CauseMachineId);

                    var productionInfo = _responseCacheService.GetProductionInfo();
                    if (productionInfo.MachineInfoList.ContainsKey(dbModel.CauseMachineId))
                    {
                        productionInfo.MachineInfoList[dbModel.CauseMachineId].Defect = activeMachine.CounterDefect;
                        await _responseCacheService.SetProductionInfo(productionInfo);
                    }

                    await HandleBoardcastingActiveProcess3M(DataTypeGroup.Waste, dbModel.ProductionPlanId
                        , dbModel.CauseMachineId, productionPlan);

                    //dole dashboard
                    //_triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.Waste);
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
        [Route("GetReportWaste3M")]
        public async Task<ProcessReponseModel<object>> GetReportWaste3M(string planId, int machineId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetReportWaste3M(planId, machineId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }
    }
}
