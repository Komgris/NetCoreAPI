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

        [Route("api/[controller]/ListByDate")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<RecordMaintenancePlanModel>>> ListByDate(DateTime date)
        {
            var output = new ProcessReponseModel<List<RecordMaintenancePlanModel>>();
            try
            {
                output.Data = await _recordMaintenancePlanService.ListByDate(date);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<RecordMaintenancePlanModel>> Create([FromBody]RecordMaintenancePlanModel data)
        {
            var output = new ProcessReponseModel<RecordMaintenancePlanModel>();
            try
            {
                await _recordMaintenancePlanService.Create(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [Route("api/[controller]/Update")]
        [HttpPut]
        public async Task<ProcessReponseModel<RecordMaintenancePlanModel>> Update([FromBody]RecordMaintenancePlanModel data)
        {
            var output = new ProcessReponseModel<RecordMaintenancePlanModel>();
            try
            {
                await _recordMaintenancePlanService.Update(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }


        [Route("api/[controller]/Get")]
        [HttpGet]
        public async Task<ProcessReponseModel<RecordMaintenancePlanModel>> Get(int id)
        {
            var output = new ProcessReponseModel<RecordMaintenancePlanModel>();
            try
            {
                output.Data = await _recordMaintenancePlanService.Get(id);
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