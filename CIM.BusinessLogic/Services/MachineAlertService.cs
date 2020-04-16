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
        private IActiveProductionPlanService _activeProductionPlanService;

        public MachineAlertService(
            IResponseCacheService responseCacheService,
            IProductionPlanService productionPlanService,
            IActiveProductionPlanService activeProductionPlanService
            )
        {
            _responseCacheService = responseCacheService;
            _productionPlanService = productionPlanService;
            _activeProductionPlanService = activeProductionPlanService;
        }
    }
}
