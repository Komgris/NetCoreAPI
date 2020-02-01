using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private IResponseCacheService _responseCacheService;

        public CacheController(
            IResponseCacheService responseCacheService
            )
        {

            _responseCacheService = responseCacheService;
        }

        const string CACHE_KEY = "CACHE_KEY";

        [HttpGet]
        public async Task<int> Get(int id)
        {
            var cached = await _responseCacheService.GetAsync(CACHE_KEY);
            var cachedNum = int.Parse(cached);
            var sum = id + cachedNum;
            await _responseCacheService.SetAsync(CACHE_KEY, sum);
            return sum;
        }
    }
}