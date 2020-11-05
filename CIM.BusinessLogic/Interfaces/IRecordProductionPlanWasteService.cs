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
        Task<PagingModel<RecordProductionPlanWasteNonePrimeModel>> NonePrimeOutputList(string keyword, int page, int howmany);
        Task<List<RecordProductionPlanWasteNonePrimeModel>> NonePrimeOutputListByMonth(int month, int year);
        Task<PagingModel<RecordProductionPlanWasteNonePrimeModel>> NonePrimeOutputListByDate(DateTime date, int page, int howmany);
        Task Delete(int id);
        Task Update(RecordProductionPlanWasteModel model);
        Task NonePrimeCreate(List<RecordProductionPlanWasteNonePrimeModel> models);
        Task<DataTable> RecordNonePrimeList(string planId, int routeId);
        Task<List<RecordProductionPlanWasteModel>> ListByMonth(int month, int year, string planId, int? routeId = null);
        Task<PagingModel<RecordProductionPlanWasteModel>> ListByDate(DateTime date, string keyword, int page, int howmany, string planId, int? routeId = null);
        Task<List<RecordProductionPlanWasteModel>> List3M(string planId, int? machineId, string keyword);
        Task<RecordProductionPlanWasteModel> Get3M(int id);
        Task<RecordProductionPlanWasteModel> Create3M(RecordProductionPlanWasteModel model);
        Task Update3M(RecordProductionPlanWasteModel model);
        Task Delete3M(int id);
    }
}
