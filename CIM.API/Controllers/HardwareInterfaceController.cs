using System;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [ApiController]
    public class HardwareInterfaceController : ControllerBase
    {
        private IMachineTagsService _callMachineTagsService;

        public HardwareInterfaceController(
            IMachineTagsService callMachineTagsService
            )
        {
            _callMachineTagsService = callMachineTagsService;
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
            //FoCusDev
            try
            {
                output = await _callMachineTagsService.Get();
            }
            catch (Exception ex)
            {
                output = ex.ToString();
            }
            return output;
        }
    }
}