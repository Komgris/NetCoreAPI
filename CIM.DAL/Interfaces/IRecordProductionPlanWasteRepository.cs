using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRecordProductionPlanWasteRepository : IRepository<RecordProductionPlanWaste, RecordProductionPlanWasteModel>
    {
        Task<List<RecordProductionPlanWasteModel>> ListByLoss(int recordManufacturingLossId);
        Task DeleteByLoss(int id);
        Task<RecordProductionPlanWaste> Get(int id);
    }
}
