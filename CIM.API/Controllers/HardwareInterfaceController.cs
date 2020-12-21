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
    public class HardwareInterfaceController : BoardcastNoSecureController
    {
        private IMachineService _machineService;
        IHardwareInterfaceService _hwinterfaceService;
        ITriggerQueueService _triggerService;


        public HardwareInterfaceController(
            IHubContext<GlobalHub> hub,
            ITriggerQueueService triggerService,
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
            _triggerService = triggerService;
        }

        //[HttpGet]
        //[Route("api/[controller]/GetMachineTags")]
        //public async Task<ProcessReponseModel<object>> GetMachineTags()
        //{
        //    var output = new ProcessReponseModel<object>();
        //    try
        //    {
        //        output.Data = JsonConvert.SerializeObject(await _machineService.GetMachineTags(), JsonsSetting);
        //        output.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        output.Message = ex.Message;
        //    }
        //    return output;
        //}

        [HttpGet]
        [Route("api/[controller]/ForceInitialTags")]
        public async Task ForceInitialTags()
        {
            await _machineService.ForceInitialTags();
        }

        [HttpPost]
        [Route("api/[controller]/SetListMachinesResetCounter")]
        public async Task SetListMachinesResetCounter([FromBody] List<int> machines, bool isCounting)
        {
            await _machineService.SetListMachinesResetCounter(machines, isCounting);
        }

        [HttpGet]
        [Route("api/[controller]/GetSystemInterfaceInfo")]
        public async Task<ProcessReponseModel<object>> GetSystemInterfaceInfo()
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                output.Data = JsonConvert.SerializeObject(await _machineService.GetSystemInterfaceInfo(), JsonsSetting);
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
        public async Task<string> SetStatus3M(int id, int statusId, bool isAuto = true)
        {
            await _activeProductionPlanService.UpdateByMachine3M(id, statusId, isAuto);
            await HandleBoardcastingActiveMachine3M(id);

            //dashboard
            var boardname = "active-process";
            var OverallDashboard = _responseCacheService.GetDashboardData(boardname);
            var output = new ChartModel();
            if (OverallDashboard == null)
            {
                OverallDashboard = _dashboardService.GetChartData(null, "sp_get_active_process", "CIMDatabase");
            }

            for(int i=0;i< OverallDashboard.Rows.Count; i++)
            {
                if(Convert.ToInt32(OverallDashboard.Rows[i]["id"]) == id)
                {
                    OverallDashboard.Rows[i]["status"] = Constans.MachineStatusString[statusId];
                    break;
                }
            }

            var chart = await Task.Run(() => JsonConvert.SerializeObject(OverallDashboard, JsonsSetting));
            output = new ChartModel
            {
                Name = boardname,
                DataString = chart,
            };

            _responseCacheService.SetDashboardData(boardname, OverallDashboard);
            await _hub.Clients.All.SendAsync(Constans.SIGNAL_R_CHANNEL.CHANNEL_DASHBOARD, JsonConvert.SerializeObject(output, JsonsSetting));

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
                await _activeProductionPlanService.UpdateMachineOutput(listData, hr);
                foreach (var item in listData)
                {
                    await HandleBoardcastingActiveMachine3M(item.MachineId);
                    //await HandleBoardcastingActiveProcess3M(DataTypeGroup.Produce, productionPlan.ProductionPlanId
                    //                                            , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);
                }

                //dole dashboard
                //_triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.ProduceCalc);

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
        public async Task<ProcessReponseModel<object>> AdditionalMachineProduce(string planId, int? machineId, int? routeId, int amount, int? hour, string remark = "")
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var productionPlan = await _activeProductionPlanService.AdditionalMachineOutput(planId, machineId, routeId, amount, hour, remark);
                if (productionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Produce, productionPlan.ProductionPlanId
                                                                , productionPlan.ActiveProcesses.Select(o => o.Key).ToArray(), productionPlan);

                    //dole dashboard
                    _triggerService.TriggerQueueing(TriggerType.CustomDashboard, (int)DataTypeGroup.ProduceCalc);
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
        [Route("api/[controller]/AdditionalMachineProduce3M")]
        public async Task<ProcessReponseModel<object>> AdditionalMachineProduce3M(string planId, int machineId, int amount, int? hour, string remark = "")
        {
            var output = new ProcessReponseModel<object>();
            try
            {
                var activeMachine = await _activeProductionPlanService.AdditionalMachineOutput3M(planId, machineId, amount, hour, remark);
                await HandleBoardcastingActiveMachine3M(machineId);

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
        public async Task UpdateNetworkStatus([FromBody] List<NetworkStatusModel> listData, bool isReset)
        {
            try
            {
                await _hwinterfaceService.UpdateNetworkStatus(listData, isReset);
            }
            catch
            {
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

        [HttpGet]
        [Route("api/[controller]/GetProductInfo")]
        public async Task<ProcessReponseModel<ProductionInfoModel>> GetProductInfo()
        {
            var output = new ProcessReponseModel<ProductionInfoModel>();
            try
            {
                output.Data = await _machineService.GetProductionInfoCache();
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