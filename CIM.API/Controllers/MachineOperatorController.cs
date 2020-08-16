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
using Microsoft.Extensions.Configuration;
using static CIM.Model.Constans;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineOperatorController : BoardcastController
    {
        private IMachineOperatorService _machineOperatorService;
        private IProductionPlanService _productionPlanService;
        private IMachineService _machineService;

        public MachineOperatorController(IHubContext<GlobalHub> hub,
            IProductionPlanService productionPlanService,
            IResponseCacheService responseCacheService,
            IDashboardService dashboardService,
            IConfiguration config,
            IActiveProductionPlanService activeProductionPlanService,
            IMachineOperatorService machineOperatorService,
            IMachineService machineService
            ) : base(hub, responseCacheService, dashboardService, config, activeProductionPlanService)
        {
            _machineOperatorService = machineOperatorService;
            _productionPlanService = productionPlanService;
            _machineService = machineService;
        }

        [HttpPost]
        public async Task<ProcessReponseModel<MachineOperatorModel>> Create(MachineOperatorModel model)
        {
            var output = new ProcessReponseModel<MachineOperatorModel>();
            try
            {
                await _machineOperatorService.Create(model);
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpPut]
        public async Task<ProcessReponseModel<MachineOperatorModel>> Update(MachineOperatorModel model)
        {
            var output = new ProcessReponseModel<MachineOperatorModel>();
            try
            {
                await _machineOperatorService.Update(model);
                var listRoute = _machineService.GetCached(model.MachineId).Result.RouteIds.ToArray();

                var channelKey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{model.PlanId}";
                var activeProductionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(channelKey);
                // Production plan of this component doesn't started yet
                if (activeProductionPlan != null)
                {
                    await HandleBoardcastingActiveProcess(DataTypeGroup.Operators, model.PlanId
                    ,listRoute, activeProductionPlan);
                }
                output.IsSuccess = true;
            }
            catch (Exception ex)
            {
                output.Message = ex.Message;
            }

            return output;
        }

        [HttpDelete]
        public async Task<ProcessReponseModel<MachineOperatorModel>> Delete(int id)
        {
            var output = new ProcessReponseModel<MachineOperatorModel>();
            try
            {
                await _machineOperatorService.Delete(id);
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
