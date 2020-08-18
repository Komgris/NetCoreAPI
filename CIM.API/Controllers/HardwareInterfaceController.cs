using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.HubConfig;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    [ApiController]
    public class HardwareInterfaceController : BoardcastController {
        private IMachineService _machineService;
        IHardwareInterfaceService _hwinterfaceService;


        public HardwareInterfaceController(
            IHubContext<GlobalHub> hub,
            IMachineService machineService,
            IResponseCacheService responseCacheService,
            IDashboardService dashboardService,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService,
            IHardwareInterfaceService hwinterfaceService
            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
            _machineService = machineService;
            _hwinterfaceService = hwinterfaceService;
        }

        [HttpGet]
        [Route("api/[controller]/GetMachineTags")]
        public async Task<ProcessReponseModel<object>> GetMachineTags()
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _machineService.GetMachineTags(), JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/ForceInitialTags")]
        public async Task ForceInitialTags()
        {
            await _machineService.ForceInitialTags();
        }

        [HttpPost]
        [Route("api/[controller]/SetListMachinesResetCounter")]
        public async Task SetListMachinesResetCounter([FromBody] List<int>  machines, bool isCounting)
        {
            await _machineService.SetListMachinesResetCounter(machines, isCounting);
        }

        [HttpGet]
        [Route("api/[controller]/CheckSystemParamters")]
        public async Task<ProcessReponseModel<object>> CheckSystemParamters()
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _machineService.CheckSystemParamters(), JsonsSetting);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }
            return output;
        }

        [HttpGet]
        [Route("api/[controller]/InitialMachineCache")]
        public async Task<string> InitialMachineCache()
        {
            try
            {
                await _machineService.InitialMachineCache();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        [HttpGet]
        [Route("api/[controller]/SetStatus")]
        public async Task<string> SetStatus(int id, int statusId, bool isAuto = true)
        {
            var productionPlan = await _activeProductionPlanService.UpdateByMachine(id, statusId, isAuto);

            // Production plan of this component doesn't started yet
            if (productionPlan != null)
            {
                var channelKey = $"{Constans.SIGNAL_R_CHANNEL_PRODUCTION_PLAN}-{productionPlan.ProductionPlanId}";
                await HandleBoardcastingActiveProcess(DataTypeGroup.Machine, productionPlan.ProductionPlanId
                    , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
            }
            return "OK";
        }

        [HttpPost]
        [Route("api/[controller]/UpdateMachineProduceCounter")]
        public async Task<ProcessReponseModel<object>> UpdateMachineProduceCounter([FromBody] List<MachineProduceCounterModel> listData, int? hour)
        {
            var hr = hour ?? DateTime.Now.Hour;
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlans = await _activeProductionPlanService.UpdateMachineOutput(listData, hr);

                foreach (var productionPlan in productionPlans)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Produce, productionPlan.ProductionPlanId
                                                                , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                }
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;

        }

        [HttpGet]
        [Route("api/[controller]/AdditionalMachineProduce")]
        public async Task<ProcessReponseModel<object>> AdditionalMachineProduce(string planId, int? machineId, int? routeId,int amount, int? hour, string remark="")
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                if (!_activeProductionPlanService.CurrentUser.IsValid)
                {
                    output.Message = "Unauthorized";
                    return output;
                }

                var productionPlan = await _activeProductionPlanService.AdditionalMachineOutput(planId, machineId, routeId, amount, hour, remark);
                if(productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Produce, productionPlan.ProductionPlanId
                                                                , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                }
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;

        }

        [HttpPost]
        [Route("api/[controller]/UpdateNetworkStatus")]
        public async Task UpdateNetworkStatus([FromBody] List<NetworkStatusModel> listData,bool isReset)
        {
            try
            {
                await _hwinterfaceService.UpdateNetworkStatus(listData, isReset);
            }
            catch {
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetNetworkStatus")]
        public async Task<ProcessReponseModel<object>> GetNetworkStatus()
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _hwinterfaceService.GetNetworkStatus(), JsonsSetting);
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