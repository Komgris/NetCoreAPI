using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Services
{
    public class MachineComponentService : IMachineComponentService
    {
        private IResponseCacheService _responseCacheService;

        public MachineComponentService(
            IResponseCacheService responseCacheService
            )
        {
            _responseCacheService = responseCacheService;
        }

        public void CreateAlert(int id)
        {
            var key = GetKey(id);

            _responseCacheService.SetAsync()

        }

        private string GetKey(int id)
        {
            return $"{Constans.RedisKey.PRODUCTION_PLAN}:{id}";
        }
    }
}
