using System;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using static CIM.Model.Constans;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers {

    [ApiController]
    public class ReportController : BaseController {

        private IReportService _service;
        private IHubContext<DashboardHub> _hub;

        public ReportController(
            IHubContext<DashboardHub> hub,
            IReportService reportService)
        {
            _hub = hub;
            _service = reportService;
        }

        #region Cim-Oper Production overview

        [HttpGet]
        [Route("api/[controller]/GetProductionSummary")]
        public async Task<ProcessReponseModel<object>> GetProductionSummary(string planId, int routeId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionSummary(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }

            return output;

        }

        [HttpGet]
        [Route("api/[controller]/GetProductionPlanInfomation")]
        public async Task<ProcessReponseModel<object>> GetProductionPlanInfomation(string planId, int routeId) {
            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionPlanInfomation(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionOperators")]
        public async Task<ProcessReponseModel<object>> GetProductionOperators(string planId, int routeId) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionOperators(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionEvents")]
        public async Task<ProcessReponseModel<object>> GetProductionEvents(string planId, int routeId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionEvents(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetCapacityUtilisation")]
        public async Task<ProcessReponseModel<object>> GetCapacityUtilisation(string planId, int routeId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetCapacityUtilisation(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        #region Cim-Oper Mc-Loss

        [HttpGet]
        [Route("api/[controller]/GetProductionLoss")]
        public async Task<ProcessReponseModel<object>> GetProductionLoss(string planId, int routeId, int lossLv, int? machineId) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionWCMLoss(planId, routeId, lossLv, machineId, null, null), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionLossHistory")]
        public async Task<ProcessReponseModel<object>> GetProductionLossHistory(string planId, int routeId, int page) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionWCMLossHistory(planId, routeId, null, null, page), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineSpeed")]
        public async Task<ProcessReponseModel<object>> GetMachineSpeed(string planId, int routeId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMachineSpeed(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        #region Cim-Oper dashboard

        [HttpGet]
        [Route("api/[controller]/GetProductionDasboard")]
        public async Task<ProcessReponseModel<object>> GetProductionDasboard(string planId, int routeId, int machineId) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionDasboard(planId, routeId, machineId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion

        #region Cim-oper waste

        [HttpGet]
        [Route("api/[controller]/GetWasteByMaterials")]
        public async Task<ProcessReponseModel<object>> GetWasteByMaterials(string planId, int routeId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteByMaterials(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetWasteByCases")]
        public async Task<ProcessReponseModel<object>> GetWasteByCases(string planId, int routeId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteByCases(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetWasteByMachines")]
        public async Task<ProcessReponseModel<object>> GetWasteByMachines(string planId, int routeId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteByMachines(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetWasteCostByTime")]
        public async Task<ProcessReponseModel<object>> GetWasteCostByTime(string planId, int routeId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteCostByTime(planId, routeId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetWasteHistory")]
        public async Task<ProcessReponseModel<object>> GetWasteHistory(string planId, int routeId, int page, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetWasteHistory(planId, routeId, from, to, page), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }
        #endregion

        #region Cim-oper mc status

        [HttpGet]
        [Route("api/[controller]/GetActiveMachineInfo")]
        public async Task<ProcessReponseModel<object>> GetActiveMachineInfo(string planId, int routeId) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActiveMachineInfo(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetActiveMachineEvents")]
        public async Task<ProcessReponseModel<object>> GetActiveMachineEvents(string planId, int routeId) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetActiveMachineEvents(planId, routeId), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineStatusHistory")]
        public async Task<ProcessReponseModel<object>> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetMachineStatusHistory(howMany, page,  planId, routeId, machineId, from, to), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }

        #endregion


        #region Cim-Mng dashboard
        public void BoardcastTrigger(DashboardType type,string channel)
        {
            var boardcastData = new BoardcastDashboardModel(type);
            try
            {
                //generating boardcast data 
                //oee
                boardcastData.Dashboard.Add(_service.GetDashboardOEE(type));
                //avr
                boardcastData.Dashboard.Add(_service.GetDashboardAvailability(type));
                //qcr
                boardcastData.Dashboard.Add(_service.GetDashboardQuality(type));
                //mpr
                boardcastData.Dashboard.Add(_service.GetDashboardPerformance(type));
                //output
                boardcastData.Dashboard.Add(_service.GetDashboardOutput(type));
                //waste
                boardcastData.Dashboard.Add(_service.GetDashboardWaste(type));
                //mcloss
                boardcastData.Dashboard.Add(_service.GetDashboardMachineLoss(type));
                //utilization
                boardcastData.Dashboard.Add(_service.GetDashboardUtilizationTime(type));

                HandleBoardcastData(boardcastData, channel);//channel: management, operation
            }
            catch (Exception ex)
            {
                boardcastData.IsSuccess = false;
                boardcastData.Message = ex.Message;
                boardcastData.Dashboard = null;
            }
        }
        private void HandleBoardcastData(BoardcastDashboardModel model, string channel)
        {
            if (model != null)
            {
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL_DASHBOARD}-{channel}";
                var dataString = JsonConvert.SerializeObject(model, JsonsSetting);
                _hub.Clients.All.SendAsync(channel, dataString);
            }
        }

        #endregion
    }
}