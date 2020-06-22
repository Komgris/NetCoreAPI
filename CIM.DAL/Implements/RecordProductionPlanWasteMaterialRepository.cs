using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanWasteMaterialRepository : Repository<RecordProductionPlanWasteMaterials, object>, IRecordProductionPlanWasteMaterialRepository
    {
        public RecordProductionPlanWasteMaterialRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<List<RecordProductionPlanWasteMaterialModel>> ListByLoss(int recordManufacturingLossId)
        {
            return await _entities.RecordProductionPlanWasteMaterials.Where(x => x.Waste.RecordManufacturingLossId == recordManufacturingLossId)
                .Select(x => new RecordProductionPlanWasteMaterialModel
                {
                    Amount = x.Amount,
                    Id = x.Id,
                    MaterialId = x.MaterialId,
                    WasteId = x.WasteId,
                }).ToListAsync();

        }
    }
}
