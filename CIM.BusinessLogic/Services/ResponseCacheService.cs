using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Model;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private IDirectSqlRepository _directSqlRepository;

        public ResponseCacheService(
            IDistributedCache distributedCache,
            IDirectSqlRepository directSqlRepository
            )
        {
            _directSqlRepository = directSqlRepository;
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
        string GetKeyProductionInfo()
        {
            return $"{RedisKey.PRODUCTION}";
        }
        string GetKeyMachineInfo(int machineId)
        {
            return $"{RedisKey.MACHINE_INFO}:{machineId}";
        }
        string GetKeyDashboard(string chartId)
        {
            return $"{RedisKey.Dashboard}:{chartId}";
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

        public ProductionInfoModel GetProductionInfo()
        {
            if (BaseService.baseProductionInfo !=null)
            {
                return BaseService.baseProductionInfo;
            }
            else
            {
                var key = GetKeyProductionInfo();
                var cache = GetAsTypeAsync<ProductionInfoModel>(key).Result;
                if (cache is null)
                {
                   return  new ProductionInfoModel();
                }
                else
                {
                    return cache;
                }
            }
        }

        public async Task SetProductionInfo(ProductionInfoModel model)
        {
            BaseService.baseProductionInfo = model;
            var key = GetKeyProductionInfo();
            await SetAsync(key, model);
        }
        public DataTable GetDashboardData(string chartId)
        {
            if (BaseService.baseDashboard.ContainsKey(chartId))
            {
                return BaseService.baseDashboard[chartId];
            }
            else
            {
                var key = GetKeyDashboard(chartId);
                return GetAsTypeAsync<DataTable>(key).Result;
            }
        }

        public async Task<DataTable> UpdateDashboardData(string chartId, int machineId, Dictionary<string,object> data)
        {
            var Cache = GetDashboardData(chartId);
            if (Cache == null)
            {
                Cache = _directSqlRepository.ExecuteSPWithQuery("sp_get_active_process",null, "CIMDatabase");
            }

            for (int i = 0; i < Cache.Rows.Count; i++)
            {
                if (Convert.ToInt32(Cache.Rows[i]["id"]) == machineId)
                {
                    foreach(var item in data)
                    {
                        Cache.Rows[i][item.Key] = item.Value;
                    }
                    break;
                }
            }
            await SetDashboardData(chartId, Cache);
            return Cache;
        }
        public async Task SetDashboardData(string chartId, DataTable data)
        {
            BaseService.baseDashboard[chartId] = data;
            var key = GetKeyDashboard(chartId);
            await SetAsync(key, data);
        }

    }
}
