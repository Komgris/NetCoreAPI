using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    //[MiddlewareFilter(typeof(CustomAuthenticationMiddlewarePipeline))]
    [ApiController]
    public class EmployeesController : BaseController
    {
        private IEmployeesService _service;
        private IUtilitiesService _utilitiesService;
        public EmployeesController(
            IEmployeesService service,
            IUtilitiesService utilitiesService)
        {
            _service = service;
            _utilitiesService = utilitiesService;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<EmployeesModel>> Create([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<EmployeesModel>();
            try
            {
                var list = JsonConvert.DeserializeObject<EmployeesModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "employees",false);
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"employees/{list.Image}";
                }
                await _service.Create(list);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<EmployeesModel>> Update([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<EmployeesModel>();

            try
            {
                var list = JsonConvert.DeserializeObject<EmployeesModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "employees", false);
                }
                else if(list.Image != "" && list.Image != null)
                {
                    list.Image = $"employees/{list.Image}";
                }
                await _service.Update(list);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/List")]
        public async Task<ProcessReponseModel<PagingModel<EmployeesModel>>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<EmployeesModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howMany, isActive);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<EmployeesModel>> Get(int id)
        {
            var output = new ProcessReponseModel<EmployeesModel>();
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

        [HttpGet]
        [Route("api/[controller]/GetFromEmployeeNo")]
        public async Task<ProcessReponseModel<EmployeesModel>> GetFromEmployeeNo(string no)
        {
            var output = new ProcessReponseModel<EmployeesModel>();
            try
            {
                output.Data = await _service.GetFromEmployeeNo(no);
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