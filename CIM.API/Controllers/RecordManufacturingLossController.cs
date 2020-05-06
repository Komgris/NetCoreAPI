using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordManufacturingLossController : BaseController
    {
        private IRecordManufacturingLossService _recordManufacturingLossService;

        public RecordManufacturingLossController(
            IRecordManufacturingLossService recordManufacturingLossService
            )
        {
            _recordManufacturingLossService = recordManufacturingLossService;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<object>> Create(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _recordManufacturingLossService.Create(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPut]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<object>> Update(RecordManufacturingLossModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _recordManufacturingLossService.Update(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("api/[controller]/GetByGuid")]
        public async Task<ProcessReponseModel<RecordManufacturingLossModel>> GetByGuid(Guid guid)
        {
            var output = new ProcessReponseModel<RecordManufacturingLossModel>();
            try
            {
                await _recordManufacturingLossService.GetByGuid(guid);
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