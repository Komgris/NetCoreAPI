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
        public string GetProductionSummary(string planid, int routeid, DateTime? from=null, DateTime? to=null) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionSummary(planid, routeid, from, to));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionPlanInfomation")]
        public string GetProductionPlanInfomation(string planid, int routeid) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionPlanInfomation(planid, routeid));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionOperators")]
        public string GetProductionOperators(string planid, int routeid) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionOperators(planid, routeid));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionEvents")]
        public string GetProductionEvents(string planid, int routeid, DateTime? from = null, DateTime? to = null) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionEvents(planid, routeid, from, to));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineSpeed")]
        public string GetMachineSpeed(string planid, int routeid, DateTime? from = null, DateTime? to = null) {
            try {
                return JsonConvert.SerializeObject(_service.GetMachineSpeed(planid, routeid, from, to));
            }
            catch (Exception e) {
                throw e;
            }
        }

        #endregion

        #region Cim-Oper Mc-Loss
        [HttpGet]
        [Route("api/[controller]/GetProductionLoss")]
        public string GetProductionLoss(string planid, int routeid,int losslv) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionWCMLoss(planid, routeid,losslv,null,null,null));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionLoss")]
        public string GetProductionLossHistory(string planid, int routeid) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionWCMLoss(planid, routeid, null, null, null, null));
            }
            catch (Exception e) {
                throw e;
            }
        }
        #endregion
    }
}
