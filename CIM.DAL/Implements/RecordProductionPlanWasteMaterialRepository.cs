using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanWasteMaterialRepository : Repository<RecordProductionPlanWasteMaterials>, IRecordProductionPlanWasteMaterialRepository
    {
        public RecordProductionPlanWasteMaterialRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<List<RecordProductionPlanWasteMaterialModel>> ListByLoss(int lossId)
        {
            throw new NotImplementedException();
        }
    }
}
