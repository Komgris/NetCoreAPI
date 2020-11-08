using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using Microsoft.AspNetCore.Http;
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
        private IProductionPlanRepository _productionPlanRepository;

        public CacheController(
            IActiveProductionPlanService activeProductionPlanService,
            IMachineService machineService,
            IResponseCacheService responseCacheService,
            IProductionPlanRepository productionPlanRepository
            )
        {
            _activeProductionPlanService = activeProductionPlanService;
            _machineService = machineService;
            _responseCacheService = responseCacheService;
            _productionPlanRepository = productionPlanRepository;
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
                        //foreach (var route in plan?.ActiveProcesses)
                        //{

                        //foreach (var machine in route.Value.Route.MachineList)
                        //{
                        //    route.Value.BoardcastData = null;

                        //    exportModel.Machines.Add(machine.Value);
                        //}
                        //}

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
        
        [HttpGet]
        [Route("DeleteProductionPlans")]
        public async Task<string> DeleteProductionPlan(string planIds)
        {
            var planList = planIds.Split(',');
            try
            {
                var data = _productionPlanRepository.Where(x => planList.Contains(x.PlanId) && x.StatusId == 3).Select(Io => Io.PlanId).ToList();
                foreach (var plan in data)
                {
                    await _activeProductionPlanService.RemoveCached(plan);
                }
                return $"OK Remove: { String.Join(',',data)}";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        [HttpPost]
        [Route("SetProductionPlans")]
        public async Task SetProductionPlans(IFormFile file)
        {

            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }

            var data = result.ToString();

            var model = JsonConvert.DeserializeObject<CacheExportModel>(data);
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
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}