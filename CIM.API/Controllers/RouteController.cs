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
    public class RouteController : BaseController
    {
        private IResponseCacheService _responseCacheService;
        private IRouteService _routeService;


        public RouteController(
            IResponseCacheService responseCacheService,
            IRouteService routeService,
            IMasterDataService masterDataService
        )
        {
            _responseCacheService = responseCacheService;
            _routeService = routeService;
            _masterDataService = masterDataService;
        }

        [Route("api/[controller]/List")]
        [HttpGet]
        public async Task<ProcessReponseModel<PagingModel<RouteListModel>>> List(string keyword = "", int page = 1, int howmany = 10,bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<RouteListModel>>();
            try
            {
                output.Data = await _routeService.List(keyword, page, howmany, isActive);
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
                // todo
                //var currentUser = (CurrentUserModel)HttpContext.Items[Constans.CURRENT_USER];
                output.Data = await _routeService.Get(id);
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
                await _routeService.Update(data);
                await _masterDataService.Refresh(Constans.MasterDataType.Machines);
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
                await _routeService.Create(data);
                await _masterDataService.Refresh(Constans.MasterDataType.Machines);
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