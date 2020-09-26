using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class RecordActiveProductionPlanRepository : Repository<RecordActiveProductionPlan, object>, IRecordActiveProductionPlanRepository
    {
        public RecordActiveProductionPlanRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
}
