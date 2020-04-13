﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers {

    [ApiController]
    [Route("api/[controller]/action")]
    public class ReportController : ControllerBase {

        private IReportService _service;

        public ReportController(IReportService reportService) {
            _service = reportService;
        }

        #region Cim-Oper Production overview


        //[HttpGet]
        //public string Get() {
        //    return "";
        //}

        [HttpGet]
        [Route("api/[controller]/GetProductionSummary")]
        public string GetProductionSummary(int planid, int routeid, DateTime? from=null, DateTime? to=null) {
            try {
                return JsonConvert.SerializeObject(_service.GetProductionSummary(planid, routeid, from, to));
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionPlanInfomation")]
        public string GetProductionPlanInfomation(int planid, int routeid) {
            try {
                return _service.GetProductionPlanInfomation(planid, routeid);
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        public string GetProductionOperators(int planid, int routeid) {
            try {
                return _service.GetProductionOperators(planid, routeid);
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetProductionEvents")]
        public string GetProductionEvents(int planid, int routeid, DateTime? from = null, DateTime? to = null) {
            try {
                return _service.GetProductionEvents(planid, routeid, from, to);
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineSpeed")]
        public string GetMachineSpeed(int planid, int routeid, DateTime? from = null, DateTime? to = null) {
            try {
                return _service.GetMachineSpeed(planid, routeid, from, to);
            }
            catch (Exception e) {
                throw e;
            }
        }

        #endregion
    }
}
