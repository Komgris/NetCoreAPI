using System;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : BaseController
    {
        private IActiveProductionPlanService _activeProductionPlanService;
        private IMachineService _machineService;
        private IResponseCacheService _responseCacheService;

        public CacheController(
            IActiveProductionPlanService activeProductionPlanService,
            IMachineService machineService,
            IResponseCacheService responseCacheService
            )
        {
            _activeProductionPlanService = activeProductionPlanService;
            _machineService = machineService;
            _responseCacheService = responseCacheService;
        }

        [HttpGet]
        public async Task<string> Get(string cacheKey)
        {
            var cached = await _responseCacheService.GetAsync(cacheKey);
            return cached;
        }

        [HttpPost]
        public async Task<string> Add(int id, string data, string key)
        {
            var cacheKey = $"{key}{id}";
            await _responseCacheService.SetAsync(cacheKey, data);
            return "OK";
        }

        [HttpGet]
        [Route("GetProductionPlans")]
        public async Task<string> GetProductionPlan(string planIds)
        {

            var planList = planIds.Split(',');
            var exportModel = new CacheExportModel();
            try
            {
                foreach (var planId in planList)
                {
                    var plan = await _activeProductionPlanService.GetCached(planId);
                    if (plan != null)
                    {
                        foreach (var route in plan?.ActiveProcesses)
                        {
                            foreach (var machine in route.Value.Route.MachineList)
                            {
                                exportModel.Machines.Add(machine.Value);
                            }
                        }

                        exportModel.ProductionPlans.Add(plan);
                    }
                }

                return JsonConvert.SerializeObject(exportModel, JsonsSetting);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("SetProductionPlans")]
        public async Task<object> SetProductionPlans(CacheExportModel model)
        {

            try
            {
                foreach (var plan in model.ProductionPlans)
                {
                    await _activeProductionPlanService.SetCached(plan);
                }

                foreach (var machine in model.Machines)
                {
                    await _machineService.SetCached(machine.Id,machine);
                }
                return "OK";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}