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
        Task<ActiveMachine3MModel> GetCached3M(int id);
        Task RemoveCached(int id, ActiveMachineModel model);
        Task SetCached(int id, ActiveMachineModel model);
        Task SetCached3M(ActiveMachine3MModel model);
        Task<Dictionary<int, ActiveMachineModel>> BulkCacheMachines(string productionPlanId, int routeId, Dictionary<int, ActiveMachineModel> machineList);
        Task<ActiveMachine3MModel> BulkCacheMachines3M(string productionPlanId, int machineId);
        Task<List<RouteMachineModel>> GetMachineByRoute(int routeId);
        Task InsertMappingRouteMachine(List<RouteMachineModel> data);
        //Task<List<MachineTagsModel>> GetMachineTags();
        Task SetListMachinesResetCounter(List<int> machines, bool isCounting);
        Task SetMachinesResetCounter3M(int machineId, bool isCounting);
        Task<SystemInterfaceModel> GetSystemInterfaceInfo();
        Task ForceInitialTags();
        Task InitialMachineCache();
        Task<ProductionInfoModel> GetProductionInfoCache();
        Task SetProductInfoCache(ProductionInfoModel info);
        MachineInfoModel GetProductInfoData(string planId);
    }
}
