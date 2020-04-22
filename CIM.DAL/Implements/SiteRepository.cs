using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class SiteRepository : Repository<Sites>, ISiteRepository
    {
        public SiteRepository(cim_dbContext context) : base(context)
        {

        }

        public async Task<IList<SiteModel>> ExecuteProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            return await execStoreProcedure<SiteModel>(procedureName, parameters);
        }

    }
}
