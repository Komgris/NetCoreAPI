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
    public class AccidentController : ControllerBase
    {
        private IAccidentService _accidentService;

        public AccidentController(
            IAccidentService accidentService)
        {
            _accidentService = accidentService;
        }

        [HttpGet]
        public async Task<ProcessReponseModel<AccidentModel>> Get(int id)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                output.Data = await _accidentService.Get(id);
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
