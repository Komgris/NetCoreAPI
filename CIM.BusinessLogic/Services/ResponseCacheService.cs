using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.Model;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public ResponseCacheService(
            IDistributedCache distributedCache
            )
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetAsTypeAsync<T>(string key)
        {
            try
            {
                var jsonString = await GetAsync(key) ?? string.Empty;
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing master data to object. " + ex.Message);
            }

        }

        public async Task<string> GetAsync(string key)
        {
            return await _distributedCache.GetStringAsync(key);
        }

        public async Task SetAsync(string key, object model)
        {
            if (model == null)
            {
                _distributedCache.Remove(key);
            }
            else
            {
                var stringJson = JsonConvert.SerializeObject(model);
                await _distributedCache.SetStringAsync(key, stringJson);
            }
        }
        public async Task SetAsyncExpire(string key, object model, int ttlMin)
        {
            if (model == null)
            {
                _distributedCache.Remove(key);
            }
            else
            {
                var option = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = new TimeSpan(0, ttlMin, 0)
                };
                var stringJson = JsonConvert.SerializeObject(model);
                await _distributedCache.SetStringAsync(key, stringJson, option);
            }
        }
        public async Task RemoveAsync(string key)
        {
            _distributedCache.Remove(key);
        }

        string GetKeyPlan(string planId)
        {
            return $"{RedisKey.ACTIVE_PRODUCTION_PLAN}:{planId}";
        }
        string GetKeyMachine(int machineId)
        {
            return $"{RedisKey.MACHINE}:{machineId}";
        }

        public async Task SetActivePlan(ActiveProductionPlan3MModel model)
        {
            if (model.Status != PRODUCTION_PLAN_STATUS.Finished)
            {
                BaseService.baseListActive[model.ProductionPlanId] = model;
                var key = GetKeyPlan(model.ProductionPlanId);
                await SetAsync(key, model);
            }
        }

        public ActiveProductionPlan3MModel GetActivePlan(string planId)
        {
            if (BaseService.baseListActive.ContainsKey(planId))
            {
                return BaseService.baseListActive[planId];
            }
            else
            {
                var key = GetKeyPlan(planId);
                return GetAsTypeAsync<ActiveProductionPlan3MModel>(key).Result;
            }
        }

        public void RemoveActivePlan(string planId)
        {
            BaseService.baseListActive.Remove(planId);

            var key = GetKeyPlan(planId);
            RemoveAsync(key).Wait();
        }

        public ActiveMachine3MModel GetActiveMachine(int machineId)
        {
            if (BaseService.baseListMachine.ContainsKey(machineId))
            {
                return BaseService.baseListMachine[machineId];
            }
            else
            {
                var key = GetKeyMachine(machineId);
                return GetAsTypeAsync<ActiveMachine3MModel>(key).Result;
            }
        }

        public async Task SetActiveMachine(ActiveMachine3MModel model)
        {
            BaseService.baseListMachine[model.Id] = model;
            var key = GetKeyMachine(model.Id);
            await SetAsync(key, model);
        }

    }
}
