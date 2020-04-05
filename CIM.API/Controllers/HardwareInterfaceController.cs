using System;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HardwareInterfaceController : ControllerBase
    {
        private IHardwareInterfaceService _service;

        public HardwareInterfaceController(IHardwareInterfaceService service)
        {
            _service = service;
        }

        [HttpPut]
        //[Route("api/[controller]/Create")]
        public async Task<bool> UpdateStatus([FromBody]MachineStatusModel model)
        {
            try
            {
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                return await _service.UpdateStatus(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut]
        //[Route("api/[controller]/Create")]
        public async Task<bool> UpdateOutput([FromBody]RecordOutputModel model)
        {
            try
            {
                // todo
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                return await _service.UpdateOutput(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}