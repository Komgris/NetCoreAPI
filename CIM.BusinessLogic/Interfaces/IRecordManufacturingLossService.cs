using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordManufacturingLossService : IBaseService
    {
        Task<ActiveProductionPlanModel> Create(RecordManufacturingLossModel model);
        Task<ActiveProductionPlanModel> Update(RecordManufacturingLossModel model);
        Task<RecordManufacturingLossModel> GetByGuid(Guid guid);
        Task<ActiveProductionPlanModel> End(RecordManufacturingLossModel model);
    }
}
