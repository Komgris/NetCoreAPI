using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IActiveProcessService
    {
        string GetKey(string productionPlanId, int routeId);
        Task<ActiveProcessModel> GetCached(string id, int routeId);
        Task SetActiveProcess(ActiveProcessModel model);
        Task RemoveCached(string id, int routeId);
    }
}
