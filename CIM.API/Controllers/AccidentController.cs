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
        [Route("List")]
        public async Task<ProcessReponseModel<PagingModel<AccidentModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<AccidentModel>>();
            try
            {
                output.Data = await _accidentService.List(keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
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

        [HttpPost]
        public async Task<ProcessReponseModel<AccidentModel>> Create(AccidentModel model)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _accidentService.Create(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPut]
        public async Task<ProcessReponseModel<AccidentModel>> Update(AccidentModel model)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _accidentService.Update(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }


        [HttpDelete]
        public async Task<ProcessReponseModel<AccidentModel>> Delete(int id)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _accidentService.Delete(id);
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
