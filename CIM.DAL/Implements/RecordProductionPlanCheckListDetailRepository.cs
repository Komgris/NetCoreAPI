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
    public class RecordProductionPlanCheckListDetailRepository : Repository<RecordProductionPlanCheckListDetail, RecordProductionPlanCheckListDetailModel>, IRecordProductionPlanCheckListDetailRepository
    {
        public RecordProductionPlanCheckListDetailRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }

        public async Task<RecordProductionPlanCheckListDetail> Get(int id)
        {
            var dbModel = await _dbset.Where(x => x.Id == id).Select(x => new RecordProductionPlanCheckListDetail
            {
                Id = x.Id,
                RecordCheckListId = x.RecordCheckListId,
                CheckListId = x.CheckListId,
                IsCheck = x.IsCheck,
                Remark = x.Remark,
                CheckList = new ProductionPlanCheckList
                {
                    Id =x.Id,
                    CheckListTypeId = x.CheckList.CheckListTypeId
                }
            }).FirstOrDefaultAsync();
            return dbModel;
        }
    }
}
