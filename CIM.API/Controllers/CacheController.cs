using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
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


    }
}