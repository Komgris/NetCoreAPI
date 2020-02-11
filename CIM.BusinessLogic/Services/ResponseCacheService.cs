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
        public async Task<string> GetAsync(string key)
        {
            return await _distributedCache.GetStringAsync(key);
        }

        public async Task SetAsync(string key, object model)
        {
            var stringJson = JsonConvert.SerializeObject(model);
            await _distributedCache.SetStringAsync(key, stringJson);
        }
    }
}
