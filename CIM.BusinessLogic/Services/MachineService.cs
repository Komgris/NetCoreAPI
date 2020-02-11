using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Services
{
    public class MachineService : IMachineService
    {
        private readonly IResponseCacheService _responseCacheService;

        public MachineService(
            IResponseCacheService responseCacheService)
        {
            _responseCacheService = responseCacheService;

        }

        public List<MachineCacheModel> ListCached()
        {
            return new List<MachineCacheModel>();
        }
    }
}
