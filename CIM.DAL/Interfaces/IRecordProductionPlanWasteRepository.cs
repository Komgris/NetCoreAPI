using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRecordProductionPlanWasteRepository : IRepository<RecordProductionPlanWaste>
    {
        Task<List<RecordProductionPlanWasteModel>> ListByLoss(int id);
        Task DeleteByLoss(int id);
    }
}
