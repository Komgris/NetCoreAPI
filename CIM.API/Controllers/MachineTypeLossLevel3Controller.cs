using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System.Collections.Generic;

namespace CIM.API.Controllers
{
    [ApiController]
    public class MachineTypeLossLevel3Controller : ControllerBase
    {
        private IMachineTypeLossLevel3Service _service;
        public MachineTypeLossLevel3Controller(
            IMachineTypeLossLevel3Service service
        )
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<MachineTypeLossLevel3ListModel>>> List(int? machineTypeId, int? lossLevel3Id, int page = 1, int howmany = 15)
        {
            var output = new ProcessReponseModel<PagingModel<MachineTypeLossLevel3ListModel>>();
            try
            {
                output.Data = await _service.List(machineTypeId, lossLevel3Id, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

        [Route("api/[controller]/Update")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> Update([FromBody] List<MachineTypeLossLevel3Model> data, int machineTypeId)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.Update(machineTypeId, data);

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