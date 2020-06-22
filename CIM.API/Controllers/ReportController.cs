using System;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using static CIM.Model.Constans;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers {

    [ApiController]
    public class ReportController : BaseController {

        public ReportController(
            IResponseCacheService responseCacheService,
            IHubContext<GlobalHub> hub,
            IReportService reportService) 
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
            _service = reportService;
        }

        #region Cim-Oper Production overview

        [HttpGet]
        [Route("api/[controller]/GetProductionSummary")]
        public async Task<ProcessReponseModel<object>> GetProductionSummary(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionSummary(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }

            return output;

        }

        [HttpGet]
        [Route("api/[controller]/GetProductionPlanInfomation")]
        public async Task<ProcessReponseModel<object>> GetProductionPlanInfomation(string planId, int routeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionPlanInfomation(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionOperators")]
        public async Task<ProcessReponseModel<object>> GetProductionOperators(string planId, int routeId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionOperators(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionEvents")]
        public async Task<ProcessReponseModel<object>> GetProductionEvents(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionEvents(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetCapacityUtilisation")]
        public async Task<ProcessReponseModel<object>> GetCapacityUtilisation(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetCapacityUtilisation(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        #region Cim-Oper Mc-Loss

        [HttpGet]
        [Route("api/[controller]/GetProductionLoss")]
        public async Task<ProcessReponseModel<object>> GetProductionLoss(string planId, int routeId, int lossLv, int? machineId, int? lossId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionWCMLoss(planId, routeId, lossLv, machineId, lossId, null, null), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionLossHistory")]
        public async Task<ProcessReponseModel<object>> GetProductionLossHistory(string planId, int routeId, int page)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionWCMLossHistory(planId, routeId, null, null, page), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineSpeed")]
        public async Task<ProcessReponseModel<object>> GetMachineSpeed(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMachineSpeed(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        #region Cim-Oper dashboard

        [HttpGet]
        [Route("api/[controller]/GetProductionDasboard")]
        public async Task<ProcessReponseModel<object>> GetProductionDasboard(string planId, int routeId, int machineId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionDasboard(planId, routeId, machineId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        #region Cim-oper waste

        [HttpGet]
        [Route("api/[controller]/GetWasteByMaterials")]
        public async Task<ProcessReponseModel<object>> GetWasteByMaterials(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteByMaterials(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetWasteByCases")]
        public async Task<ProcessReponseModel<object>> GetWasteByCases(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteByCases(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetWasteByMachines")]
        public async Task<ProcessReponseModel<object>> GetWasteByMachines(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteByMachines(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetWasteCostByTime")]
        public async Task<ProcessReponseModel<object>> GetWasteCostByTime(string planId, int routeId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteCostByTime(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetWasteHistory")]
        public async Task<ProcessReponseModel<object>> GetWasteHistory(string planId, int routeId, int page, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteHistory(planId, routeId, from, to, page)));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }
        #endregion

        #region Cim-oper mc status

        [HttpGet]
        [Route("api/[controller]/GetActiveMachineInfo")]
        public async Task<ProcessReponseModel<object>> GetActiveMachineInfo(string planId, int routeId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActiveMachineInfo(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetActiveMachineEvents")]
        public async Task<ProcessReponseModel<object>> GetActiveMachineEvents(string planId, int routeId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActiveMachineEvents(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineStatusHistory")]
        public async Task<ProcessReponseModel<object>> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMachineStatusHistory(howMany, page, planId, routeId, machineId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        #region Cim-Mng Realtime process

        [Route("api/[controller]/GetBoardcastingDashboard")]
        [HttpGet]
        public async Task<string> GetBoardcastingDashboard(string channel)
        {
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL_DASHBOARD}-{channel}";
            return CacheForBoardcast<BoardcastModel>(await GetCached(channelKey));
        }

        [Route("api/[controller]/BoardcastingDashboard")]
        [HttpGet]
        public async Task<string> BoardcastingDashboard(DataFrame dataFrame, BoardcastType updateType, string channel)
        {
            var channelKey = $"{Constans.SIGNAL_R_CHANNEL_DASHBOARD}-{channel}";
            var boardcastData = await _service.GenerateBoardcastManagementData(dataFrame, updateType);
            if (boardcastData.Data.Count > 0)
            {
                await HandleBoardcastingManagementData(channelKey, boardcastData);
            }

            return JsonConvert.SerializeObject(boardcastData, JsonsSetting);
        }

        #endregion

        #region Cim-Oper realtime process

        [Route("api/[controller]/GetBoardcastActiveOperationData")]
        [HttpGet]
        public async Task<string> GetBoardcastActiveOperationData(string productionPlan, int routeId)
        {
            var channelKey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var activeProductionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(channelKey);
            if (activeProductionPlan.ActiveProcesses[routeId]!.BoardcastData is null)
            {
                activeProductionPlan.ActiveProcesses[routeId].BoardcastData =
                    await _service.GenerateBoardcastData(BoardcastType.All, productionPlan, routeId);
            }

            return JsonConvert.SerializeObject(activeProductionPlan, JsonsSetting);
        }

        [Route("api/[controller]/BoardcastingActiveOperationData")]
        [HttpGet]
        public async Task<string> BoardcastingActiveOperationData(BoardcastType updateType, string productionPlan, int routeId)
        {
            var channelKey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan}";
            var activeProductionPlan = await GetCached<ActiveProductionPlanModel>(channelKey);
            if (activeProductionPlan!.ActiveProcesses[routeId] != null)
            {
                return JsonConvert.SerializeObject(
                    await HandleBoardcastingActiveProcess(updateType, productionPlan, routeId, activeProductionPlan));
            }
            return "";
        }

        #endregion

        #region Cim-Mng Report
        [Route("api/[controller]/OEEReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> OEEReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetOEEReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/OutPutReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> GetOutputReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetOutputReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/WasteReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> WasteReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/MachineLossReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> MachineLossReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMachineLossReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/QualityReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> QualityReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetQualityReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/SPCReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> SPCReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetSPCReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/ElectricityReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> ElectricityReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetElectricityReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/ProductionSummaryReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> ProductionSummaryReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionSummaryReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/OperatingTimeReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>>OperatingTimeReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetOperatingTimeReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/ActualDesignSpeedReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> ActualDesignSpeedReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActualDesignSpeedReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/MaintenanceReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> MaintenanceReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMaintenanceReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/CostAnalysisReport")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> CostAnalysisReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetCostAnalysisReport(data)));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        #endregion
    }
}