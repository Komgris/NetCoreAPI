using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineOperatorController : BaseController
    {
        private IMachineOperatorService _machineOperatorService;

        public MachineOperatorController(
            IMachineOperatorService machineOperatorService
            )
        {
            _machineOperatorService = machineOperatorService;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<MachineOperatorModel>> Create(MachineOperatorModel model)
        {
            var output = new ProcessReponseModel<MachineOperatorModel>();
            try
            {
                await _machineOperatorService.Create(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPut]
        public async Task<ProcessReponseModel<MachineOperatorModel>> Update(MachineOperatorModel model)
        {
            var output = new ProcessReponseModel<MachineOperatorModel>();
            try
            {
                await _machineOperatorService.Update(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpDelete]
        public async Task<ProcessReponseModel<MachineOperatorModel>> Delete(int id)
        {
            var output = new ProcessReponseModel<MachineOperatorModel>();
            try
            {
                await _machineOperatorService.Delete(id);
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
