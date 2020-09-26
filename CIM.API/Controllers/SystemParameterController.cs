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

namespace CIM.API.Controllers
{
    [ApiController]
    public class SystemParameterController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private ISystemParameterService _service;

        public SystemParameterController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            ISystemParameterService service,
            IMasterDataService masterDataService
        )
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
            _service = service;
            _masterDataService = masterDataService;
        }

        [Route("api/[controller]/Create")]
        [HttpPost]
        public async Task<ProcessReponseModel<SystemParameterModel>> Create([FromBody] SystemParameterModel data)
        {
            var output = new ProcessReponseModel<SystemParameterModel>();
            try
            {
                await _service.Create(data);
                await RefreshMasterData(Constans.MasterDataType.SystemParameter);
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
        public async Task<ProcessReponseModel<SystemParameterModel>> Update([FromBody] SystemParameterModel data)
        {
            var output = new ProcessReponseModel<SystemParameterModel>();
            try
            {
                await _service.Update(data);
                await RefreshMasterData(Constans.MasterDataType.SystemParameter);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/Delete")]
        [HttpDelete]
        public async Task<ProcessReponseModel<object>> Delete(int id)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.Delete(id);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.IsSuccess = false;
                output.Message = ex.ToString();
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<ProcessReponseModel<SystemParameterModel>> Get(int id)
        {
            var output = new ProcessReponseModel<SystemParameterModel>();
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

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<SystemParameterModel>>> List(string keyword = "", int page = 1, int howMany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<SystemParameterModel>>();
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
    }
}