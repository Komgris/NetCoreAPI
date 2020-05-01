using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using CIM.DAL.Utility;

namespace CIM.DAL.Implements
{
    public class ComponentTypeLossLevel3Repository : Repository<ComponentTypeLossLevel3>, IComponentTypeLossLevel3Repository
    {
        private IDirectSqlRepository _directSqlRepository;

        public ComponentTypeLossLevel3Repository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

    }
}
