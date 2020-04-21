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
            IList<SiteModel> data = null;
            var proc = _entities.LoadStoredProc(procedureName);
            foreach (var item in parameters)
            {
                proc.AddParam(item.Key, item.Value);
            }

            await proc.ExecAsync(x => Task.Run(() => data = x.ToList<SiteModel>()));
            return data;
        }

    }
}
