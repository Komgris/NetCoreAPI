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
        Task<PagingModel<RecordManufacturingLossModel>> List(string planId, int? routeId, string keyword, int page, int howmany);
        Task<List<RecordManufacturingLossModel>> ListByMonth(int month, int year, string planId, int? routeId = null);
        Task<PagingModel<RecordManufacturingLossModel>> ListByDate(DateTime date, string keyword, int page, int howmany, string planId, int? routeId = null);
        Task<PagingModel<RecordManufacturingLossModel>> List3M(string planId, bool isAuto, string keyword, int page, int howmany);
    }
}
