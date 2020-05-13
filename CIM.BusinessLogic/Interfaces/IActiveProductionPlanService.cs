using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IActiveProductionPlanService
    {
        string GetKey(string productionPlanId);
        Task<ActiveProductionPlanModel> GetCached(string id);
        Task SetCached(ActiveProductionPlanModel model);
        Task RemoveCached(string id); 
        Task<bool> Start(string planId, int route, int? target);
        Task Finish(ProductionPlanModel model);
        Task Pause(string id, int routeId);
        Task Resume(string id, int routeId);
    }

}
