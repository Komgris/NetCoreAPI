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
    public class RouteController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private IRouteService _service;

        public RouteController(
            IHubContext<GlobalHub> hub,
            IResponseCacheService responseCacheService,
            IRouteService service,
            IMasterDataService masterDataService
        )
        {
            _hub = hub;
            _responseCacheService = responseCacheService;
            _service = service;
            _masterDataService = masterDataService;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<RouteListModel>>> List(string keyword = "", int page = 1, int howmany = 10, bool isActive = true, int? processTypeId = null)
        {
            var output = new ProcessReponseModel<PagingModel<RouteListModel>>();
            try
            {
                output.Data = await _service.List(keyword, page, howmany, isActive, processTypeId);
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
        public async Task<ProcessReponseModel<RouteListModel>> Get(int id)
        {
            var output = new ProcessReponseModel<RouteListModel>();
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

        [Route("api/[controller]/Update")]
        [HttpPut]
        public async Task<ProcessReponseModel<RouteListModel>> Update([FromBody] RouteListModel data)
        {
            var output = new ProcessReponseModel<RouteListModel>();
            try
            {
                await _service.Update(data);
                await  RefreshMasterData(Constans.MasterDataType.Machines);
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
        public async Task<ProcessReponseModel<RouteListModel>> Create([FromBody] RouteListModel data)
        {
            var output = new ProcessReponseModel<RouteListModel>();
            try
            {
                await _service.Create(data);
                await  RefreshMasterData(Constans.MasterDataType.Machines);
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