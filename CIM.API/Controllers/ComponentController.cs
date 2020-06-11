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
    [ApiController]
    public class ComponentController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private IComponentService _componentService;

        public ComponentController(
        IResponseCacheService responseCacheService,
        IComponentService componentService
    )
        {
            _responseCacheService = responseCacheService;
            _componentService = componentService;
        }


        [Route("api/[controller]/GetByMachine")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<ComponentModel>>> GetByMachineType(int machineId)
        {
            var output = new ProcessReponseModel<List<ComponentModel>>();
            try
            {
                output.Data = await _componentService.GetComponentByMachine(machineId);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<ComponentModel>>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<ComponentModel>>();
            try
            {
                output.Data = await _componentService.List(keyword, page, howMany, isActive);
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
        public async Task<ProcessReponseModel<ComponentModel>> Get(int id)
        {
            var output = new ProcessReponseModel<ComponentModel>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _componentService.Get(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<ComponentModel>> Create([FromBody] ComponentModel data)
        {
            var output = new ProcessReponseModel<ComponentModel>();
            try
            {
                await _componentService.Create(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Update")]
        [HttpPut]
        public async Task<ProcessReponseModel<ComponentModel>> Update([FromBody] ComponentModel data)
        {
            var output = new ProcessReponseModel<ComponentModel>();
            try
            {
                await _componentService.Update(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/InsertMappingMachineComponent")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> InsertMappingMachineComponent([FromBody] MappingMachineComponent data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _componentService.InsertMappingMachineComponent(data);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/GetComponentNoMachineId")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<ComponentModel>>> GetComponentNoMachineId(string keyword = "")
        {
            var output = new ProcessReponseModel<List<ComponentModel>> ();
            try
            {
                output.Data = await _componentService.GetComponentNoMachineId(keyword);
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