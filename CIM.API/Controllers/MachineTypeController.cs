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
        private IHubContext<MachineHub> _hub;
        private IResponseCacheService _responseCacheService;
        private IMachineTypeService _machineTypeService;
        private IUtilitiesService _utilitiesService;

        public MachineTypeController(
            IHubContext<MachineHub> hub,
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
                list.Image = await _utilitiesService.UploadImage(file, "componentType");
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
                list.Image = await _utilitiesService.UploadImage(file, "componentType");
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
        public async Task<ProcessReponseModel<PagingModel<MachineTypeModel>>> List(string keyword = "", int page = 1, int howmany = 10)
        {
            var output = new ProcessReponseModel<PagingModel<MachineTypeModel>>();
            try
            {
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _machineTypeService.List(keyword, page, howmany);
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
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
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