using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRecordProductionPlanWasteRepository : IRepository<RecordProductionPlanWaste>
    {
        void DeleteByLoss(int id);
        Task<List<RecordProductionPlanWaste>> ListByLoss(int id);
    }
}
