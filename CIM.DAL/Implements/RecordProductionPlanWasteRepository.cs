using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CIM.Model;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanWasteRepository : Repository<RecordProductionPlanWaste>, IRecordProductionPlanWasteRepository
    {
        public RecordProductionPlanWasteRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {

        }

        public async Task DeleteByLoss(int lossId)
        {
            var model = await _dbset.Where(x => x.RecordManufacturingLossId == lossId).Select(x => new
            {
                Waste = x,
                Masterials = x.RecordProductionPlanWasteMaterials
            }).ToListAsync();

            foreach (var item in model)
            {
                foreach (var material in item.Masterials)
                {
                    _entities.Remove(item);
                }
                _entities.Remove(item);
            }
        }

        public async Task<List<RecordProductionPlanWasteModel>> ListByLoss(int id)
        {
            throw new NotImplementedException();
        }

    }
}