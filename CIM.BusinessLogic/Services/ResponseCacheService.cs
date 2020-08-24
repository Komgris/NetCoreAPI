using CIM.BusinessLogic.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if(model == null)
            {
                _distributedCache.Remove(key);
            }
            else
            {
                var stringJson = JsonConvert.SerializeObject(model);
                await _distributedCache.SetStringAsync(key, stringJson);
            }
        }
        public async Task SetAsyncExpire(string key, object model,int ttlMin)
        {
            if (model == null)
            {
                _distributedCache.Remove(key);
            }
            else
            {
                var option = new DistributedCacheEntryOptions(){
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
    }
}
