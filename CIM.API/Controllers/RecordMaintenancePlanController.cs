using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    public class RecordMaintenancePlanController : BaseController
    {
        private IRecordMaintenancePlanService _recordMaintenancePlanService;

        public RecordMaintenancePlanController(
                IRecordMaintenancePlanService recordMaintenancePlanService
            )
        {
            _recordMaintenancePlanService = recordMaintenancePlanService;
        }

        [Route("api/[controller]/ListByMonth")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<RecordMaintenancePlanModel>>> ListByMonth(int month, int year)
        {
            var output = new ProcessReponseModel<List<RecordMaintenancePlanModel>>();
            try
            {
                output.Data = await _recordMaintenancePlanService.ListByMonth(month, year);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }
    }
}