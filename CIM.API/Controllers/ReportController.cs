using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers {

    [ApiController]
    public class ReportController : ControllerBase {

        private IReportService _service;

        public ReportController(IReportService reportService) {
            _service = reportService;
        }

        #region Cim-Oper Production overview

        [HttpGet]
        [Route("api/[controller]/GetProductionSummary")]
        public string GetProductionSummary(string planId, int routeId, DateTime? from=null, DateTime? to=null) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionSummary(planId, routeId, from, to));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionPlanInfomation")]
        public string GetProductionPlanInfomation(string planId, int routeId) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionPlanInfomation(planId, routeId));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionOperators")]
        public string GetProductionOperators(string planId, int routeId) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionOperators(planId, routeId));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionEvents")]
        public string GetProductionEvents(string planId, int routeId, DateTime? from = null, DateTime? to = null) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionEvents(planId, routeId, from, to));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineSpeed")]
        public string GetMachineSpeed(string planId, int routeId, DateTime? from = null, DateTime? to = null) {
            try {
                return JsonConvert.SerializeObject(_service.GetMachineSpeed(planId, routeId, from, to));
            }
            catch (Exception e) {
                throw e;
            }
        }

        #endregion

        #region Cim-Oper Mc-Loss
        [HttpGet]
        [Route("api/[controller]/GetProductionLoss")]
        public string GetProductionLoss(string planId, int routeId,int lossLv,int? mcId) {
            try {
                if (lossLv == 0) return "";
                return JsonConvert.SerializeObject(_service.GetProductionWCMLoss(planId, routeId,lossLv, mcId, null,null));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionLossHistory")]
        public string GetProductionLossHistory(string planId, int routeId) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionWCMLoss(planId, routeId, null, null, null, null));
            }
            catch (Exception e) {
                throw e;
            }
        }
        #endregion

        #region MyRegion  Cim-Oper dashboard

        [HttpGet]
        [Route("api/[controller]/GetProductionDasboard")]
        public string GetProductionDasboard(string planId, int routeId, int mcId) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionDasboard(planId, routeId, mcId));
            }
            catch (Exception e) {
                throw e;
            }
        }
        #endregion
    }
}
