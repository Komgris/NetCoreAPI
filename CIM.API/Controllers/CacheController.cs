using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Cache;
using CIM.BusinessLogic.Interfaces;
using CIM.Model;
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

        [HttpGet]
        public async Task<string> Get(int id, string key)
        {
            var cacheKey = $"{key}{id}";
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


    }
}