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
    }
}