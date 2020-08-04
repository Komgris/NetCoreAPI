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
        Task<ActiveProductionPlanModel> Start(string planId, int route, int? target);
        Task<ActiveProductionPlanModel> Finish(string planId, int route);
        Task<ActiveProductionPlanModel> Pause(string planId, int route,int lossLevel3);
        Task<ActiveProductionPlanModel> Resume(string planId, int route);
        Task<ActiveProductionPlanModel> UpdateByMachine(int id, int statusId, bool isAuto);
        Task<List<ActiveProductionPlanModel>> UpdateMachineOutput(List<MachineProduceCounterModel> listData, int hour);
        Task<ActiveProductionPlanModel> AdditionalMachineOutput(string planId, int machineId, int amount, int hour, string remark);
        Task<int[]> ListMachineReady(string productionPlanId);
        Task<int[]> ListMachineLossRecording(string productionPlanId);
        Task<int[]> ListMachineLossAutoRecording(string productionPlanId);
    }

}
