using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanOutputRepository : Repository<RecordProductionPlanOutput, object>, IRecordProductionPlanOutputRepository
    {
        public RecordProductionPlanOutputRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {
        }
    }
}
