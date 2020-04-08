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
        private IHardwareInterfaceService _service;
        public HardwareInterfaceController(IHardwareInterfaceService service)
        {
            _service = service;
        }

        //[HttpPut]
        //[Route("api/[controller]/UpdateStatus")]
        //public async Task<bool> UpdateStatus([FromBody]MachineStatusModel model)
        //{
        //    throw new NotImplementedException();

        //}

        [HttpPut]
        [Route("api/[controller]/UpdateOutput")]
        public async Task<bool> UpdateOutput([FromBody]RecordOutputModel model)
        {
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                return await _service.OutputUpdate(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}