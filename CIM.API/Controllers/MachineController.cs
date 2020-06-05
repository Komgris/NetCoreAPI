using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{

    [ApiController]
    public class MachineController : BaseController
    {
        private IMachineService _service;
        public MachineController(
            IHubContext<GlobalHub> hub,
            IProductionPlanService productionPlanService,
            IMachineService service
        )
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<object>> Create([FromBody]MachineModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {

                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                await _service.Create(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpPost]
        [Route("api/[controller]/Update")]
        public async Task<ProcessReponseModel<object>> Update([FromBody]MachineModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {

                _service.CurrentUser = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" };

                await _service.Update(model);
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
        public async Task<ProcessReponseModel<PagingModel<MachineListModel>>> List(string keyword = "", int page = 1, int howMany = 10, bool isActive = true)
        {
            var output = new ProcessReponseModel<PagingModel<MachineListModel>>();
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
        public async Task<ProcessReponseModel<MachineListModel>> Get(int id)
        {
            var output = new ProcessReponseModel<MachineListModel>();
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

        [HttpGet]
        [Route("api/[controller]/GetMachineByRoute")]
        public async Task<ProcessReponseModel<List<MachineModel>>> GetMachineByRoute(int routeId)
        {
            var output = new ProcessReponseModel<List<MachineModel>>();
            try
            {

                output.Data = await _service.GetMachineByRoute(routeId);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [Route("api/[controller]/InsertMappingRouteMachine")]
        [HttpPost]
        public async Task<ProcessReponseModel<object>> InsertMappingRouteMachine([FromBody] List<RouteMachineModel> data)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.InsertMappingRouteMachine(data);
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