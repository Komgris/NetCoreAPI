﻿using CIM.DAL.Interfaces;
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
    public class RecordProductionPlanWasteRepository : Repository<RecordProductionPlanWaste, RecordProductionPlanWasteModel>, IRecordProductionPlanWasteRepository
    {
        public RecordProductionPlanWasteRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
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
                _entities.RemoveRange(item.Masterials);
                _entities.Remove(item.Waste);
            }
        }

        public async Task<List<RecordProductionPlanWasteModel>> ListByLoss(int recordManufacturingLossId)
        {
            return await _dbset.Where(x => x.RecordManufacturingLossId == recordManufacturingLossId)
                .Where(x=>x.IsDelete == false)
                .Select(x => new RecordProductionPlanWasteModel
                {
                    CauseMachineId = x.CauseMachineId,
                    Reason = x.Reason,
                    //RouteId = x.RouteId,
                    //WasteLevel2Id = x.WasteLevel2Id,
                    //WasteLevel1Id = x.WasteLevel2.WasteLevel1Id,
                    Id = x.Id
                }).ToListAsync();
        }

        public async Task<RecordProductionPlanWaste> Get(int id)
        {
            var dbModel = await _dbset.Where(x => x.Id == id).Select(x => new
            {
                Waste = x,
                Masterials = x.RecordProductionPlanWasteMaterials
            }).FirstOrDefaultAsync();
            dbModel.Waste.RecordProductionPlanWasteMaterials = dbModel.Masterials;
            return dbModel.Waste;
        }

    }
}