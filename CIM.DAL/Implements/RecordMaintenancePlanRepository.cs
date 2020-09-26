using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class RecordMaintenancePlanRepository : Repository<RecordMaintenancePlan, RecordMaintenancePlanModel>, IRecordMaintenancePlanRepository
    {
        public RecordMaintenancePlanRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {
        }
    }
}
