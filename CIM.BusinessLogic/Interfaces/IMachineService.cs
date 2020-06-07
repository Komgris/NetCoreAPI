using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMachineService : IBaseService
    {
        List<MachineCacheModel> ListCached();
        Task<PagingModel<MachineListModel>> List(string keyword, int page, int howMany, bool isActive);
        Task Create(MachineListModel model);
        Task Update(MachineListModel model);
        Task<MachineListModel> Get(int id);
        string CachedKey(int id);
        Task<ActiveMachineModel> GetCached(int id);
        Task RemoveCached(int id, ActiveMachineModel model);
        Task SetCached(int id, ActiveMachineModel model);
        Task<Dictionary<int, ActiveMachineModel>> BulkCacheMachines(string productionPlanId, int routeId, Dictionary<int, ActiveMachineModel> machineList);
        Task<List<RouteMachineModel>> GetMachineByRoute(int routeId);
        Task InsertMappingRouteMachine(List<RouteMachineModel> data);

        Task<List<MachineTagsModel>> GetMachineTags();
    }
}
