using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordProductionPlanWasteService : IBaseService
    {
        Task<RecordProductionPlanWasteModel> Get(int id);
        Task<List<RecordProductionPlanWasteModel>> ListByLoss(int id);
        Task<RecordProductionPlanWasteModel> Create(RecordProductionPlanWasteModel model);
        Task<PagingModel<RecordProductionPlanWasteModel>> List(string planId, int? routeId, string keyword, int page, int howmany);
        Task Delete(int id);
        Task Update(RecordProductionPlanWasteModel model);
        Task NonePrimeCreate(List<RecordProductionPlanWasteNonePrimeModel> models);
        Task<DataTable> RecordNonePrimeList(string planId, int routeId);
    }
}
