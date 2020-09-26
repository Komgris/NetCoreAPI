using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class SystemParameterRepository : Repository<SystemParameter, SystemParameterModel>, ISystemParameterRepository
    {
        public SystemParameterRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }

    }
}
