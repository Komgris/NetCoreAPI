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
    public class RecordProductionPlanInformationRepository : Repository<RecordProductionPlanInformation, RecordProductionPlanInformationModel>, IRecordProductionPlanInformationRepository
    {
        public RecordProductionPlanInformationRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }
        public async Task<RecordProductionPlanInformation> Get(int id)
        {
            var dbModel = await _dbset.Where(x => x.Id == id).Select(x => new
            {
                informationList = x,
                detail = x.RecordProductionPlanInformationDetail
            }).FirstOrDefaultAsync();
            dbModel.informationList.RecordProductionPlanInformationDetail = dbModel.detail;
            return dbModel.informationList;
        }
    }
}
