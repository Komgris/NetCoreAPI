using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [ApiController]
    public class HardwareInterfaceController : BaseController
    {
        private IMachineService _machineService;

        public HardwareInterfaceController(
            IMachineService machineService
            )
        {
            _machineService = machineService;
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineTags")]
        public async Task<ProcessReponseModel<object>> GetMachineTags()
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _machineService.GetMachineTags(), JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/ForceInitialTags")]
        public async Task ForceInitialTags()
        {
            await _machineService.ForceInitialTags();
        }

        [HttpPost]
        [Route("api/[controller]/SetListMachinesResetCounter")]
        public async Task SetListMachinesResetCounter([FromBody] List<int>  machines)
        {
            await _machineService.SetListMachinesResetCounter(machines);
        }

        [HttpGet]
        [Route("api/[controller]/CheckSystemParamters")]
        public async Task<ProcessReponseModel<object>> CheckSystemParamters()
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _machineService.CheckSystemParamters(), JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/InitialMachineCache")]
        public async Task<string> InitialMachineCache()
        {
            try
            {
                await _machineService.InitialMachineCache();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}