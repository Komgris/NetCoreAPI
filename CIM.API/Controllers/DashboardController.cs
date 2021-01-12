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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : BoardcastNoSecureController
    {
        public DashboardController(
        IResponseCacheService responseCacheService,
        IHubContext<GlobalHub> hub,
        IDashboardService dashboardService,
        IConfiguration config,
        IActiveProductionPlanService activeProductionPlanService
        ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
        }


        [HttpGet]
        public async Task<string> ExecBoardCastQueue(TriggerType trigtype)
        {
            try
            {

                ////var Q = _dashboardService.GetBoardcastQueue(trigtype);
                ////foreach (var q in Q)
                ////{
                switch (trigtype)
                {
                    case TriggerType.CustomDashboard:
                       foreach(var chartId in Dashboard3MConfig.Keys)
                        {
                            var OverallDashboard = await _dashboardService.GenerateCustomDashboard3M(chartId);
                            var chart = await Task.Run(() => JsonConvert.SerializeObject(OverallDashboard, JsonsSetting));
                            var output = new ChartModel
                            {
                                Name = chartId,
                                DataString = chart,
                            };
                            await _hub.Clients.All.SendAsync(Constans.SIGNAL_R_CHANNEL.CHANNEL_DASHBOARD, JsonConvert.SerializeObject(output, JsonsSetting));
                        }
                        break;
                    case TriggerType.ActiveProcess:
                        await BoardcastingActiveOperation3M();
                    break;
                    case TriggerType.CalcHour:
                        break;
                }
                ////}
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        #region Management

        [HttpGet]
        public async Task<string> GetDashboardCached(DashboardCachedCH cachedCH)
        {
            return CacheForBoardcast<ProductionDataModel>(await GetCached(CachedCHKey(cachedCH)));
        }

        [HttpGet]
        public async Task<string> BoardcastingCustomDashboard(DataTypeGroup updateType)
        {
            var output = "";
            try
            {
                var boardcastData = await _dashboardService.GenerateCustomDashboard(updateType);
                if (boardcastData?.UnitData.Count > 0)
                {
                    await HandleBoardcastingData(CachedCHKey(DashboardCachedCH.Dole_Custom_Dashboard), boardcastData);
                }
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetStandardManagementDashboard(DataFrame timeFrame)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _dashboardService.GetManagementDashboard(timeFrame), JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.ToString();
            }
            return output;
        }

        #endregion

        #region Active Operation

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionDasboard(string planId, int routeId, int machineId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetProductionDasboard(planId, routeId, machineId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<string> GetActiveBoardcastCached(string productionPlan, int routeId)
        {
            var channelKey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var activeProductionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(channelKey);
            if (activeProductionPlan != null && activeProductionPlan.ActiveProcesses.ContainsKey(routeId) && activeProductionPlan.ActiveProcesses[routeId].BoardcastData == null)
            {
                activeProductionPlan.ActiveProcesses[routeId].BoardcastData =
                    await _dashboardService.GenerateBoardcast(DataTypeGroup.All, productionPlan, routeId);
            }

            return JsonConvert.SerializeObject(activeProductionPlan, JsonsSetting);
        }
        [HttpGet]
        public async Task<string> GetActiveBoardcastCached3M(string productionPlan, int machineId)
        {
            var activeProductionPlan = _responseCacheService.GetActivePlan(productionPlan);
            if (activeProductionPlan != null)
            {
                activeProductionPlan.ProductionData =
                    await _dashboardService.GenerateBoardcast(DataTypeGroup.All, productionPlan, machineId);
            }

            return JsonConvert.SerializeObject(activeProductionPlan, JsonsSetting);
        }
        [HttpGet]
        public async Task<string> GetActiveMachineBoardcastCached3M(int machineId)
        {
            var activeMachine = _responseCacheService.GetActiveMachine(machineId);   
            return JsonConvert.SerializeObject(activeMachine, JsonsSetting);
        }

        [HttpGet]
        public async Task<string> BoardcastingActiveOperation(DataTypeGroup updateType, string productionPlan, int routeId)
        {
            var channelKey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var activeProductionPlan = await GetCached<ActiveProductionPlanModel>(channelKey);
            if (activeProductionPlan!.ActiveProcesses[routeId] != null)
            {
                return JsonConvert.SerializeObject(
                    await HandleBoardcastingActiveProcess(updateType, productionPlan, new int[] { routeId }, activeProductionPlan));
            }
            return "OK";
        }

        [HttpGet] 
        public async Task<ActiveProductionPlan3MModel> BoardcastingActiveOperation3M(string planId = null)
        {
            try
            {
                var activeList = await _dashboardService.GenerateOperationBroadcast3M();
                //Boardcast activeprocess
                foreach(var item in activeList)
                {
                    var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}:{item.ProductionPlanId}";
                    await BoardcastClientData(channelKey, item);
                }
                return activeList.FirstOrDefault(x=>x.ProductionPlanId == planId);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        #endregion

        #region apart of Production data

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionSummary(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetProductionSummary(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }

            return output;

        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionPlanInfomation(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetProductionPlanInfomation(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionOperators(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetProductionOperators(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionEvents(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetProductionEvents(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetCapacityUtilisation(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetCapacityUtilisation(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        //curl -X GET "https://localhost:44365/api/Dashboard/GetChartData?chartData=sp_get_active_process&sourceData=CIMDatabaseDashboard" -H "accept: text/plain"
        [HttpGet]
        public async Task<object> GetChartData(DateTime? datestamp, string chartData, string sourceData)
        {
            var param = new Dictionary<string, object>
            {
                {"@datestamp",  datestamp}
            };

            return await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetChartData(param, chartData, sourceData), JsonsSetting));
        }

        [HttpGet]
        public async Task<object> GetData(string parameters, string chartData = "sp_get_machine", string sourceData = "CIMDatabaseDashboard")
        {
            var paramList = JsonConvert.DeserializeObject<Dictionary<string, object>>(parameters);
            return await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetChartData(paramList, chartData, sourceData), JsonsSetting));
        }


        #endregion

        #region Waste

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetWasteByMaterials(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetWasteByMaterials(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetWasteByCases(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetWasteByCases(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetWasteByMachines(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetWasteByMachines(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetWasteCostByTime(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetWasteCostByTime(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        #region Machine & loss

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetActiveMachineInfo(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetActiveMachineInfo(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }
        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetActiveMachineEvents(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetActiveMachineEvents(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetMachineSpeed(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetMachineSpeed(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> GetProductionLoss(string planId, int routeId, int lossLv, int? machineId, int? lossId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_dashboardService.GetProductionWCMLoss(planId, routeId, lossLv, machineId, lossId, null, null), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

    }
}