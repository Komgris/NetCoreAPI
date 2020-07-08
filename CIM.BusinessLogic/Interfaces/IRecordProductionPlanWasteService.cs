using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordProductionPlanWasteService
    {
        Task<RecordProductionPlanWasteModel> Get(int id);
        Task<List<RecordProductionPlanWasteModel>> ListByLoss(int id);
        Task<RecordProductionPlanWasteModel> Create(RecordProductionPlanWasteModel model);
        Task<PagingModel<RecordProductionPlanWasteModel>> List(string planId, int? routeId, string keyword, int page, int howmany);
        Task Delete(int id);
        Task Update(RecordProductionPlanWasteModel model);
    }
}
