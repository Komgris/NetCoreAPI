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
        Task<List<MachineModel>> List();
        Task Create(MachineListModel model);
        Task Update(MachineListModel model);
        Task<MachineListModel> Get(int id);
        string CachedKey(int id);
        Task<ActiveMachineModel> GetCached(int id);
        Task RemoveCached(int id, ActiveMachineModel model);
        Task SetCached(int id, ActiveMachineModel model);
        Task<Dictionary<int, ActiveMachineModel>> BulkCacheMachines(string productionPlanId, int routeId, Dictionary<int, ActiveMachineModel> machineList);
        //Task<Dictionary<int, ActiveMachine3MModel>> BulkCacheMachines3M(string productionPlanId, int routeId, Dictionary<int, ActiveMachine3MModel> machineList);
        Task<List<RouteMachineModel>> GetMachineByRoute(int routeId);
        Task InsertMappingRouteMachine(List<RouteMachineModel> data);
        Task<List<MachineTagsModel>> GetMachineTags();
        Task SetListMachinesResetCounter(List<int> machines, bool isCounting);
        Task<SystemParametersModel> CheckSystemParamters();
        Task ForceInitialTags();
        Task InitialMachineCache();
    }
}
