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

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : BoardcastController {
        IReportService _service;

        public Formatting JsonFormatting { get; private set; }

        public ReportController(
            IResponseCacheService responseCacheService,
            IHubContext<GlobalHub> hub,
            IDashboardService dashboardService,
            IReportService service,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService
            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
            _service = service;
        }

        #region Cim-Oper Mc-Loss

        [HttpGet]
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

        [HttpGet]
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
        public async Task<ProcessReponseModel<object>> GetActiveMachineInfo(string planId, int routeId, int machineId)
        {

            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActiveMachineInfo(planId, routeId, machineId), JsonsSetting));
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

        #region Cim-Mng Report

        [HttpPost]
        public async Task<ProcessReponseModel<object>> OEEReport([FromBody] ReportDateModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetOEEReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> OutputReport([FromBody]ReportDateModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetOutputReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> WasteReport([FromBody]ReportDateModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> MachineLossReport([FromBody]ReportDateModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMachineLossReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> ORReport([FromBody] ReportDateModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetORReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> SPCReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetSPCReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> ElectricityReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetElectricityReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> ProductionSummaryReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionSummaryReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>>OperatingTimeReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetOperatingTimeReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> ActualDesignSpeedReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActualDesignSpeedReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> MaintenanceReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMaintenanceReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> CostAnalysisReport([FromBody]ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetCostAnalysisReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> HSEReport([FromBody] ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetHSEReport(data), JsonsFormatting));
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<object>> AttendantReport([FromBody] ReportTimeCriteriaModel data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetAttendantReport(data), JsonFormatting));
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