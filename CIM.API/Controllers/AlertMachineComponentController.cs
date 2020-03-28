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
    public class AlertMachineComponentController : BaseController
    {
        private IMachineComponentService _machineComponentService;

        public AlertMachineComponentController(
                IMachineComponentService machineComponentService
            )
        {
            _machineComponentService = machineComponentService;
        }


        [HttpPost]
        public async Task<ProcessReponseModel<bool>> Create(int id)
        {
            var output = new ProcessReponseModel<bool>();
            try
            {
                _machineComponentService.CreateAlert(id);
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }
            return output;
        }

    }
}