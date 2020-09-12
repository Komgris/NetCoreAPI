using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [ApiController]
    public class ComponentTypeController : BaseController
    {
        private IComponentTypeService _service;
        private IUtilitiesService _utilitiesService;


        public ComponentTypeController(
            IHubContext<GlobalHub> hub,
            IComponentTypeService service,
            IUtilitiesService utilitiesService,
            IMasterDataService masterDataService
        )
        {
            _hub = hub;
            _service = service;
            _utilitiesService = utilitiesService;
            _masterDataService = masterDataService;
        }

        [Route("api/[controller]/GetByMachineType")]
        [HttpGet]
        public async Task<ProcessReponseModel<List<ComponentTypeModel>>> GetByMachineType(int machineTypeId)
        {
            var output = new ProcessReponseModel<List<ComponentTypeModel>>();
            try
            {
                output.Data = await _service.GetComponentTypesByMachineType(machineTypeId);
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
        public async Task<ProcessReponseModel<PagingModel<ComponentTypeModel>>> List(string keyword = "", int page = 1, int howmany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<ComponentTypeModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howmany, isActive);
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
                await _service.InsertByMachineId(data);
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
        public async Task<ProcessReponseModel<ComponentTypeModel>> Create([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<ComponentTypeModel>();
            try
            {
                var list = JsonConvert.DeserializeObject<ComponentTypeModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "componentType",false);
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"componentType/{list.Image}";
                }
                await _service.Create(list);
                await RefreshMasterData(Constans.MasterDataType.ComponentType);
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
        public async Task<ProcessReponseModel<ComponentTypeModel>> Update([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<ComponentTypeModel>();
            try
            {
                var list = JsonConvert.DeserializeObject<ComponentTypeModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "componentType", false);
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"componentType/{list.Image}";
                }
                await _service.Update(list);
                await RefreshMasterData(Constans.MasterDataType.ComponentType);
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
                output.Data = await _service.Get(id);
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