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
    public class SiteRepository : Repository<Sites, object>, ISiteRepository
    {
        public SiteRepository(cim_3m_1Context context, IConfiguration configuration ) : base(context, configuration)
        {}

        public async Task<IList<SiteModel>> ExecuteProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            return await ExecStoreProcedure<SiteModel>(procedureName, parameters);
        }

    }
}
