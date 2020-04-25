using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers
{

    [ApiController]
    public class ReportController : BaseController
    {

        private IReportService _service;

        public ReportController(IReportService reportService)
        {
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
            try {
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
        public async Task<ProcessReponseModel<object>> GetProductionLossHistory(string planId, int routeId) {

            var output = new ProcessReponseModel<object>();
            try {
                output.Data = await Task.Run(() => JsonConvert.SerializeObject(_service.GetProductionWCMLoss(planId, routeId, null, null, null, null), JsonsSetting));
                output.IsSuccess = true;
            }
            catch (Exception e) {
                output.Message = e.Message;
            }
            return output;
        }
        #endregion

        #region MyRegion  Cim-Oper dashboard

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
    }
}
troller-