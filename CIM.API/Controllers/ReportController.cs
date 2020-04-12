using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIM.API.Controllers {

    [ApiController]
    [Route("api/[controller]/action")]
    public class ReportController : ControllerBase {

        private IReportService _service;

        public ReportController(IReportService reportService) {
            _service = reportService;
        }

        [HttpGet]
        public string Get(int id) {
            try {
                var now = DateTime.Now;
                return _service.GetProductionSummary(1,1,now,now);
            }
            catch (Exception e) {
                throw e;
            }
        }
    }
}
