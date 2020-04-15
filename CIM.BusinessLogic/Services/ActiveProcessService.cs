using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ActiveProcessService : IActiveProcessService
    {
        private IResponseCacheService _responseCacheService;

        public ActiveProcessService(
            IResponseCacheService responseCacheService
            )
        {
            _responseCacheService = responseCacheService;
        }

        public string GetKey(string productionPLanId, int routeId)
        {
            return $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPLanId}:{routeId}";
        }

        public async Task<ActiveProcessModel> GetCached(string id, int routeId)
        {
            var key = GetKey(id, routeId);
            return await _responseCacheService.GetAsTypeAsync<ActiveProcessModel>(key);
        }

        public async Task SetActiveProcess(ActiveProcessModel model)
        {
            await _responseCacheService.SetAsync(GetKey(model.ProductionPlanId, model.Route.Id.Value), model);
        }

        public async Task RemoveCached(string id, int routeId)
        {
            var key = GetKey(id, routeId);
            await _responseCacheService.SetAsync(key, null);
        }

    }
}
