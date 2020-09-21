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
    public class AccidentController : BaseController
    {
        private IAccidentService _service;

        public AccidentController(
            IAccidentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("List")]
        public async Task<ProcessReponseModel<PagingModel<AccidentModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<AccidentModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpGet]
        [Route("End")]
        public async Task<ProcessReponseModel<AccidentModel>> End(int id)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _service.End(id);
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
                output.Data = await _service.Get(id);
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
                await _service.Create(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
            }

            return output;
        }

        [HttpPut]
        public async Task<ProcessReponseModel<AccidentModel>> Update(AccidentModel model)
        {
            var output = new ProcessReponseModel<AccidentModel>();
            try
            {
                await _service.Update(model);
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
                await _service.Delete(id);
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
