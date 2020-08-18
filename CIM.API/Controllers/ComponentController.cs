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
        private IComponentService _service;

        public ComponentController(
        IResponseCacheService responseCacheService,
        IComponentService service
    )
        {
            _responseCacheService = responseCacheService;
            _service = service;
        }


        [Route("api/[controller]/GetByMachine")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<ComponentModel>>> GetByMachineType(int machineId)
        {
            var output = new ProcessReponseModel<List<ComponentModel>>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                output.Data = await _service.GetComponentByMachine(machineId);
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
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

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
        public async Task<ProcessReponseModel<ComponentModel>> Get(int id)
        {
            var output = new ProcessReponseModel<ComponentModel>();
            try
            {
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                output.Data = await _service.Get(id);
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
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                await _service.Create(data);
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
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                await _service.Update(data);
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
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                await _service.InsertMappingMachineComponent(data);
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
                if (!_service.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                output.Data = await _service.GetComponentNoMachineId(keyword);
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