using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanWasteRepository : Repository<RecordProductionPlanWaste>, IRecordProductionPlanWasteRepository
    {
        public RecordProductionPlanWasteRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {

        }

        public void DeleteByLoss(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<RecordProductionPlanWaste>> ListByLoss(int id)
        {
            throw new NotImplementedException();
        }
    }
}