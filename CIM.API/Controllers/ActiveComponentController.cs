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
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveComponentController : BaseController
    {
        private IResponseCacheService _responseCacheService;

        public ActiveComponentController(
            IResponseCacheService responseCacheService
            )
        {
            _responseCacheService = responseCacheService;
        }

        [HttpGet]
        public async Task<List<ActiveComponentModel>> Get(string productionPlanId)
        {
            var activeProcess = await _responseCacheService.GetAsTypeAsync<ActiveProcessModel>($"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlanId}");
            var output = new List<ActiveComponentModel>();
            foreach (var machine in activeProcess.Route.MachineList)
            {
                foreach (var component in machine.Value.Components)
                {
                    output.Add( new ActiveComponentModel {
                        MachineComponentId = component.Value.Id,
                        MachineId = component.Value.MachineId,
                        ProductionPlanId = productionPlanId,
                    });
                }
            }
            return output;

        }

    }
}