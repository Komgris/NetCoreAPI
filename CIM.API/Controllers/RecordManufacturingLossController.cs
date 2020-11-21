using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
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
    public class RecordManufacturingLossController : BoardcastController
    {
        private IRecordManufacturingLossService _service;

        public RecordManufacturingLossController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IDashboardService dashboardService,
            IConfiguration config,
            IRecordManufacturingLossService service,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<object>> Create(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlan = await _service.Create(model);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Machine, productionPlan.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);

                    //dole dashboard
                    var boardcastData = await _dashboardService.GenerateCustomDashboard(DataTypeGroup.Loss);
                    if (boardcastData?.UnitData.Count > 0)
                    {
                        await HandleBoardcastingData(CachedCHKey(DashboardCachedCH.Dole_Custom_Dashboard), boardcastData);
                    }
                }
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPost]
        [Route("api/[controller]/End")]
        public async Task<ProcessReponseModel<object>> End(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlan = await _service.End(model);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Machine, productionPlan.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);

                    //dole dashboard
                    var boardcastData = await _dashboardService.GenerateCustomDashboard(DataTypeGroup.Loss);
                    if (boardcastData?.UnitData.Count > 0)
                    {
                        await HandleBoardcastingData(CachedCHKey(DashboardCachedCH.Dole_Custom_Dashboard), boardcastData);
                    }
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
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<object>> Update(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlan = await _service.Update(model);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Machine, productionPlan.ProductionPlanId
                        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);

                    if (model.WasteList.Count > 0)
                    {
                        await HandleBoardcastingActiveProcess(DataTypeGroup.Waste, productionPlan.ProductionPlanId
                            , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                    }

                    //dole dashboard
                    var boardcastData = await _dashboardService.GenerateCustomDashboard(DataTypeGroup.Loss);
                    if (boardcastData?.UnitData.Count > 0)
                    {
                        await HandleBoardcastingData(CachedCHKey(DashboardCachedCH.Dole_Custom_Dashboard), boardcastData);
                    }
                }
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
                output.Data = await _service.GetByGuid(guid);
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
        public async Task<ProcessReponseModel<PagingModel<RecordManufacturingLossModel>>> List(string planId, int? routeId = null, string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<RecordManufacturingLossModel>>();
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

        [Route("api/[controller]/ListByMonth")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<RecordManufacturingLossModel>>> ListByMonth(int month, int year, string planId, int? routeId = null)
        {
            var output = new ProcessReponseModel<List<RecordManufacturingLossModel>>();
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

        [Route("api/[controller]/ListByDate")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<RecordManufacturingLossModel>>> ListByDate(DateTime date, string keyword, int page, int howmany, string planId, int? routeId = null)
        {
            var output = new ProcessReponseModel<PagingModel<RecordManufacturingLossModel>>();
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
        [Route("api/[controller]/List3M")]
        public async Task<ProcessReponseModel<List<RecordManufacturingLossModel>>> List3M(string planId, int machineId, bool isAuto, string keyword = "")
        {
            var output = new ProcessReponseModel<List<RecordManufacturingLossModel>>();
            try
            {
                output.Data = await _service.List3M(planId, machineId, isAuto, keyword);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetByGuid3M")]
        public async Task<ProcessReponseModel<RecordManufacturingLossModel>> GetByGuid3M(Guid guid)
        {
            var output = new ProcessReponseModel<RecordManufacturingLossModel>();
            try
            {
                output.Data = await _service.GetByGuid3M(guid);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPost]
        [Route("api/[controller]/Create3M")]
        public async Task<ProcessReponseModel<object>> Create3M(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var activeMachine = await _service.Create3M(model);
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL.CHANNEL_MACHINE}:{model.MachineId}";
                if (activeMachine != null)
                {
                    await BoardcastClientData(channelKey, activeMachine);

                    //await HandleBoardcastingActiveProcess3M(DataTypeGroup.Machine, activeMachine.ProductionPlanId, model.MachineId, activeMachine);

                    //dole dashboard
                    //var boardcastData = await _dashboardService.GenerateCustomDashboard(DataTypeGroup.Loss);
                    //if (boardcastData?.Data.Count > 0)
                    //{
                    //    await HandleBoardcastingData(CachedCHKey(DashboardCachedCH.Dole_Custom_Dashboard), boardcastData);
                    //}
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
        [Route("api/[controller]/Update3M")]
        public async Task<ProcessReponseModel<object>> Update3M(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var activeMachine = await _service.Update3M(model);
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL.CHANNEL_MACHINE}:{model.MachineId}";
                if (activeMachine != null)
                {
                    await BoardcastClientData(channelKey, activeMachine);

                    //await HandleBoardcastingActiveProcess3M(DataTypeGroup.Machine, productionPlan.ProductionPlanId
                    //    , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);

                    //if (model.WasteList.Count > 0)
                    //{
                    //    await HandleBoardcastingActiveProcess(DataTypeGroup.Waste, productionPlan.ProductionPlanId
                    //        , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                    //}

                    //dole dashboard
                    //var boardcastData = await _dashboardService.GenerateCustomDashboard(DataTypeGroup.Loss);
                    //if (boardcastData?.Data.Count > 0)
                    //{
                    //    await HandleBoardcastingData(CachedCHKey(DashboardCachedCH.Dole_Custom_Dashboard), boardcastData);
                    //}
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
        [Route("api/[controller]/Delete3M")]
        public async Task<ProcessReponseModel<object>> Delete3M(int id)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var dbModel = await _service.Get3M(id);
                await _service.Delete3M(id);
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL.CHANNEL_MACHINE}:{dbModel.MachineId}";
                var activeMachine = _responseCacheService.GetActiveMachine(dbModel.MachineId);
                if (activeMachine != null)
                {
                    await BoardcastClientData(channelKey, activeMachine);

                    //var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{dbModel.ProductionPlanId}";
                    //var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlan3MModel>(rediskey);
                    //if (productionPlan != null)
                    //{
                    //await HandleBoardcastingActiveProcess3M(DataTypeGroup.Waste, dbModel.ProductionPlanId
                    //    , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
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

        [HttpPost]
        [Route("api/[controller]/End3M")]
        public async Task<ProcessReponseModel<object>> End3M(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var activeMachine = await _service.End3M(model);
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL.CHANNEL_MACHINE}:{model.MachineId}";
                if (activeMachine != null)
                {
                    await BoardcastClientData(channelKey, activeMachine);

                    //await HandleBoardcastingActiveProcess3M(DataTypeGroup.Machine, productionPlan.ProductionPlanId
                    //    , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);

                    //dole dashboard
                    //var boardcastData = await _dashboardService.GenerateCustomDashboard(DataTypeGroup.Loss);
                    //if (boardcastData?.Data.Count > 0)
                    //{
                    //    await HandleBoardcastingData(CachedCHKey(DashboardCachedCH.Dole_Custom_Dashboard), boardcastData);
                    //}
                }
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetReportLoss3M")]
        public async Task<ProcessReponseModel<object>> GetReportLoss3M(string planId, int machineId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetReportLoss3M(planId, machineId), JsonsSetting));
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