using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class MachineAlertService : IMachineAlertService
    {
        private IResponseCacheService _responseCacheService;
        private IProductionPlanService _productionPlanService;

        public MachineAlertService(
            IResponseCacheService responseCacheService,
            IProductionPlanService productionPlanService
            )
        {
            _responseCacheService = responseCacheService;
            _productionPlanService = productionPlanService;
        }
        public async Task<MachineAlertModel> Get(string productionPlanId)
        {
            var productionPlanRouteKey = _productionPlanService.GetProductionPlanKey(productionPlanId);
            var routes = await _responseCacheService.GetAsTypeAsync<List<int>>(productionPlanRouteKey);
            foreach (var item in routes)
            {
               //var activeProcess = 
            }

            throw new NotImplementedException();
        }
    }
}
