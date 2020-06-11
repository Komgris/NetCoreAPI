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
    public class MachineTypeController : BaseController
    {
        private IMachineTypeService _machineTypeService;
        private IUtilitiesService _utilitiesService;

        public MachineTypeController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IMachineTypeService machineTypeService,
            IUtilitiesService utilitiesService
        )
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
            _machineTypeService = machineTypeService;
            _utilitiesService = utilitiesService;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]

        public async Task<ProcessReponseModel<MachineTypeModel>> Create([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<MachineTypeModel>();
            try
            {
                var list = JsonConvert.DeserializeObject<MachineTypeModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "machineType");
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"machineType/{list.Image}";
                }
                await _machineTypeService.Create(list);
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
        public async Task<ProcessReponseModel<MachineTypeModel>> Update([FromForm] IFormFile file, [FromForm] string data)
        {
            var output = new ProcessReponseModel<MachineTypeModel>();
            try
            {
                var list = JsonConvert.DeserializeObject<MachineTypeModel>(data);
                if (file != null)
                {
                    list.Image = await _utilitiesService.UploadImage(file, "machineType");
                }
                else if (list.Image != "" && list.Image != null)
                {
                    list.Image = $"machineType/{list.Image}";
                }
                await _machineTypeService.Update(list);
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
        public async Task<ProcessReponseModel<PagingModel<MachineTypeModel>>> List(string keyword = "", int page = 1, int howmany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<MachineTypeModel>>();
            try
            {
                output.Data = await _machineTypeService.List(keyword, page, howmany, isActive);
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
        public async Task<ProcessReponseModel<MachineTypeModel>> Get(int id)
        {
            var output = new ProcessReponseModel<MachineTypeModel>();
            try
            {
                output.Data = await _machineTypeService.Get(id);
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