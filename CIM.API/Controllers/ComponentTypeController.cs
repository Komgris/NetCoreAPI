using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CIM.API.Controllers
{
    [ApiController]
    public class ComponentTypeController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private IMachineService _machineService;
        private IComponentTypeService _componentTypeService;


        public ComponentTypeController(
            IResponseCacheService responseCacheService,
            IMachineService machineService,
            IComponentTypeService componentTypeService
        )
        {
            _responseCacheService = responseCacheService;
            _machineService = machineService;
            _componentTypeService = componentTypeService;
        }

        [Route("api/[controller]/GetByMachineType")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<ComponentTypeModel>>> GetByMachineType(int machineTypeId)
        {
            var output = new ProcessReponseModel<List<ComponentTypeModel>>();
            try
            {
                output.Data = await _componentTypeService.GetComponentTypesByMachineType(machineTypeId);
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
        public async Task<ProcessReponseModel<PagingModel<ComponentTypeModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<ComponentTypeModel>>();
            try
            {
                output.Data = await _componentTypeService.List(keyword, page, howmany);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/InsertMappingByMachineId")]
        [HttpPost]
        public async Task<ProcessReponseModel<ComponentTypeModel>> InsertMappingByMachineId([FromBody] MappingMachineTypeComponentTypeModel<List<ComponentTypeModel>> data)
        {
            var output = new ProcessReponseModel<ComponentTypeModel>();
            try
            {
                await _componentTypeService.InsertByMachineId(data);
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
        public async Task<ProcessReponseModel<ComponentTypeModel>> Create([FromBody] ComponentTypeModel data)
        {
            var output = new ProcessReponseModel<ComponentTypeModel>();
            try
            {
                await _componentTypeService.Create(data);
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
        public async Task<ProcessReponseModel<ComponentTypeModel>> Update([FromBody] ComponentTypeModel data)
        {
            var output = new ProcessReponseModel<ComponentTypeModel>();
            try
            {
                await _componentTypeService.Update(data);
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
        public async Task<ProcessReponseModel<ComponentTypeModel>> Get(int id)
        {
            var output = new ProcessReponseModel<ComponentTypeModel>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _componentTypeService.Get(id);
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