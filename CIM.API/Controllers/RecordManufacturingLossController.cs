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
    }
}