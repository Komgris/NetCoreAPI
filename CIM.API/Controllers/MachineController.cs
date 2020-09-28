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
            IMachineService service,
            IMasterDataService masterDataService
        )
        {
            _hub = hub;
            _service = service;
            _masterDataService = masterDataService;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ProcessReponseModel<object>> Create([FromBody]MachineListModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.Create(model);
                await RefreshMasterData(Constans.MasterDataType.Machines);
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
        public async Task<ProcessReponseModel<object>> Update([FromBody]MachineListModel model)
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                await _service.Update(model);
                await RefreshMasterData(Constans.MasterDataType.Machines);
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
        public async Task<ProcessReponseModel<List<MachineModel>>> List()
        {
            var output = new ProcessReponseModel<List<MachineModel>>();
            try
            {
                output.Data = await _service.List();
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
        public async Task<ProcessReponseModel<List<RouteMachineModel>>> GetMachineByRoute(int routeId)
        {
            var output = new ProcessReponseModel<List<RouteMachineModel>>();
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
                await RefreshMasterData(Constans.MasterDataType.Machines);
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