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
        private IRecordMaintenancePlanService _service;

        public RecordMaintenancePlanController(
                IRecordMaintenancePlanService service
            )
        {
            _service = service;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<RecordMaintenancePlanModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<RecordMaintenancePlanModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [Route("api/[controller]/ListByMonth")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<RecordMaintenancePlanModel>>> ListByMonth(int month, int year, bool isActive)
        {
            var output = new ProcessReponseModel<List<RecordMaintenancePlanModel>>();
            try
            {
                output.Data = await _service.ListByMonth(month, year, isActive);
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
                output.Data = await _service.ListByDate(date);
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
                await _service.Create(data);
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
                await _service.Update(data);
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
                output.Data = await _service.Get(id);
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