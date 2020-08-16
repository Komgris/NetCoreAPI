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
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers {

    [ApiController]
    public class ReportController : BoardcastController {
        IReportService _reposrtService;
        public ReportController(
            IResponseCacheService responseCacheService,
            IHubContext<GlobalHub> hub,
            IDashboardService dashboardService,
            IReportService reposrtService,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
            _reposrtService = reposrtService;
        }

        #region Cim-Oper Mc-Loss

        [HttpGet]
        [Route("api/[controller]/GetMachineStatusHistory")]
        public async Task<ProcessReponseModel<object>> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetMachineStatusHistory(howMany, page, planId, routeId, machineId, from, to), JsonsSetting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetProductionWCMLossHistory(planId, routeId, null, null, page), JsonsSetting));
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
        [Route("api/[controller]/GetWasteHistory")]
        public async Task<ProcessReponseModel<object>> GetWasteHistory(string planId, int routeId, int page, DateTime? from = null, DateTime? to = null)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetWasteHistory(planId, routeId, from, to, page)));
                output.IsSuccess = true;
            }
            catch (Exception e)
            {
                output.Message = e.Message;
            }
            return output;
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetOEEReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetOutputReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetWasteReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetMachineLossReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetQualityReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetSPCReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetElectricityReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetProductionSummaryReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetOperatingTimeReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetActualDesignSpeedReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetMaintenanceReport(data), JsonsFormatting));
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
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_reposrtService.GetCostAnalysisReport(data), JsonsFormatting));
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