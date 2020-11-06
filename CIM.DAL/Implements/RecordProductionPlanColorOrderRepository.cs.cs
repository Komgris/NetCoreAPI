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
    public class RecordProductionPlanColorOrderRepository : Repository<RecordProductionPlanColorOrder, RecordProductionPlanColorOrderModel>, IRecordProductionPlanColorOrderRepository
    {
        public RecordProductionPlanColorOrderRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }

        public async Task<RecordProductionPlanColorOrder> Get(int id)
        {
            var dbModel = await _dbset.Where(x => x.Id == id).Select(x => new
            {
                colorList = x,
                detail = x.RecordProductionPlanColorOrderDetail
            }).FirstOrDefaultAsync();
            dbModel.colorList.RecordProductionPlanColorOrderDetail = dbModel.detail;
            return dbModel.colorList;
        }
    }
}
