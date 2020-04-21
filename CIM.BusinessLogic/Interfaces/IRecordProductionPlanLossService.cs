using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordProductionPlanLossService : IBaseService
    {
        Task Create(ActiveProcessModel activeProcess, int machibeComponentId, int machineComponentStatusId);

        Task ManualCreate(ActiveProcessModel activeProcess, int machibeComponentId, int machineComponentStatusId);
    }
}
