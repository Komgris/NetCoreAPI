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
        Task<PagingModel<MachineListModel>> List(string keyword, int page, int howmany);
        Task Create(MachineModel model);
        Task Update(MachineModel model);
        Task<MachineListModel> Get(int id);
        string CachedKey(int id);
        Task<ActiveMachineModel> GetCached(int id);
        Task RemoveCached(int id, ActiveMachineModel model);
        Task SetCached(int id, ActiveMachineModel model);
        Task BulkCacheMachines(string productionPlanId, int routeId, Dictionary<int, MachineModel> machineList);
        Task InsertMappingRouteMachine(List<RouteMachineModel> data);

    }
}
