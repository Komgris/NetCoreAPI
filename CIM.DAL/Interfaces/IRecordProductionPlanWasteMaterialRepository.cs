using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRecordProductionPlanWasteMaterialRepository : IRepository<RecordProductionPlanWasteMaterials, object>
    {
        Task<List<RecordProductionPlanWasteMaterialModel>> ListByLoss(int lossId);
        Task<PagingModel<RecordProductionPlanWasteNonePrimeModel>> NonePrimeOutputList(string keyword, int page, int howmany);
    }
}
