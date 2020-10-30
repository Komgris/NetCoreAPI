using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IActiveProductionPlanService : IBaseService
    {
        string GetKey(string productionPlanId);
        Task<ActiveProductionPlanModel> GetCached(string id);
        Task<ActiveProductionPlan3MModel> GetCached3M(string id);
        Task SetCached(ActiveProductionPlanModel model);
        Task SetCached3M(ActiveProductionPlan3MModel model);
        Task RemoveCached(string id);
        Task<ActiveProductionPlan3MModel> Start(string planId, int route, int? target);
        Task<ActiveProductionPlanModel> Finish(string planId, int route);
        Task<ActiveProductionPlanModel> Pause(string planId, int route, int lossLevel3);
        Task<ActiveProductionPlanModel> Resume(string planId, int route);
        Task<ActiveProductionPlanModel> UpdateByMachine(int id, int statusId, bool isAuto);
        Task<ActiveProductionPlan3MModel> UpdateByMachine3M(int id, int statusId, bool isAuto);
        Task<List<ActiveProductionPlanModel>> UpdateMachineOutput(List<MachineProduceCounterModel> listData, int hour);
        Task<ActiveProductionPlanModel> AdditionalMachineOutput(string planId, int? machineId, int? routeId, int amount, int? hour, string remark);
        Task<int[]> ListMachineReady(string productionPlanId);
        Task<int[]> ListMachineLossRecording(string productionPlanId);
        Task<int[]> ListMachineLossAutoRecording(string productionPlanId);
    }

}
