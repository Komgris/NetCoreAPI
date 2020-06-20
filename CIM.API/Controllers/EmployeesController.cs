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
    public class EmployeesController : ControllerBase
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
        public async Task<EmployeesModel> Create([FromForm] IFormFile file, [FromForm] string data)
        {
            try
            {
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                var list = JsonConvert.DeserializeObject<EmployeesModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "employees");
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"employees/{list.Image}";
                }
                return await _service.Create(list);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<EmployeesModel> Update([FromForm] IFormFile file, [FromForm] string data)
        {
            try
            {
                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                var list = JsonConvert.DeserializeObject<EmployeesModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "employees");
                }
                else if(list.Image != "" && list.Image != null)
                {
                    list.Image = $"employees/{list.Image}";
                }
                return await _service.Update(list);
            }
            catch (Exception e)
            {
                throw e;
            }
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
        public async Task<EmployeesModel> Get(int id)
        {
            try
            {
                return await _service.Get(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}