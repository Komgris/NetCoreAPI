using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIM.API.Cache;
using CIM.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineCacheController : ControllerBase
    {
        private IResponseCacheService _responseCacheService;

        public MachineCacheController(
            IResponseCacheService responseCacheService
            )
        {
            _responseCacheService = responseCacheService;
        }

        //const string prefix = "machine";
        //const string machineListKey = "machine-list";

        [HttpGet]
        public async Task<string> Get(int machineId)
        {
            var cacheKey = $"{Constans.RedisKey.MACHINE}{machineId}";
            var cached = await _responseCacheService.GetAsync(cacheKey);
            return cached;
        }

        [HttpPost]
        public async Task<string> Add(int machineId, string data)
        {
            var machineList = await _responseCacheService.GetAsync(Constans.RedisKey.MACHINE_LIST) ?? "";
            var distinctArray = machineList.Split(',')
                .ToList();
            distinctArray.Add(machineId.ToString());
            var finalList = string.Join(',', distinctArray.Distinct());
            var cacheKey = $"{Constans.RedisKey.MACHINE}{machineId}";
            await _responseCacheService.SetAsync(Constans.RedisKey.MACHINE_LIST, finalList);
            await _responseCacheService.SetAsync(cacheKey, data);
            return "OK";
        }


    }
}