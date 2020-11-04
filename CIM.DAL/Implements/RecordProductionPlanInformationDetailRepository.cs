using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanInformationDetailRepository : Repository<RecordProductionPlanInformationDetail, RecordProductionPlanInformationDetailModel>, IRecordProductionPlanInformationDetailRepository
    {
        public RecordProductionPlanInformationDetailRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }
    }
}
