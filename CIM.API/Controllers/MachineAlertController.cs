using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Services;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineAlertController : BaseController
    {
        private IMachineAlertService _machineAlertService;

        public MachineAlertController(
            IMachineAlertService machineAlertService
            )
        {
            _machineAlertService = machineAlertService;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<object>> Get(string productionPlanId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var data = await _machineAlertService.Get(productionPlanId);
                output.Data = JsonConvert.SerializeObject(data, JsonsSetting);
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