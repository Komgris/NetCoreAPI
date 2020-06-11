using System;
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

        [HttpPut]
        [Route("api/[controller]/UpdateLoss")]
        public async Task<bool> UpdateLoss([FromBody]RecordLossModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("api/[controller]/UpdateStatus")]
        public async Task<bool> UpdateStatus([FromBody]MachineStatusModel model)
        {
            throw new NotImplementedException();

        }

        [HttpPut]
        [Route("api/[controller]/UpdateOutput")]
        public async Task<bool> UpdateOutput([FromBody]RecordOutputModel model)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineTags")]
        public async Task<string> GetMachineTags()
        {
            string output;
            try
            {
                output = JsonConvert.SerializeObject(await _machineService.GetMachineTags(), JsonsSetting);
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }
            return output;
        }
    }
}