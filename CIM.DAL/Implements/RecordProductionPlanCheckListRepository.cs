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
    public class RecordProductionPlanCheckListRepository : Repository<RecordProductionPlanCheckList, RecordProductionPlanCheckListModel>, IRecordProductionPlanCheckListRepository
    {
        public RecordProductionPlanCheckListRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }
        public async Task<RecordProductionPlanCheckList> Get(int id)
        {
            var dbModel = await _dbset.Where(x => x.Id == id).Select(x => new
            {
                checklist = x,
                detail = x.RecordProductionPlanCheckListDetail
            }).FirstOrDefaultAsync();
            dbModel.checklist.RecordProductionPlanCheckListDetail = dbModel.detail;
            return dbModel.checklist;
        }
    }
}
