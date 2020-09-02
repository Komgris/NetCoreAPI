using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoredProcedureEFCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class WasteLevel1Repository : Repository<WasteLevel1, object>, IWasteLevel1Repository
    {
        public WasteLevel1Repository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {

        }

        public async Task<IList<WasteDictionaryModel>> ListAsDictionary()
        {
            return await _dbset.Where(x => x.IsActive == true && x.IsDelete == false)
                .Select(x => new WasteDictionaryModel
                {
                    Id = x.Id,
                    ProcessTypeId = x.ProcessTypeId,
                    Description = x.Description,
                    Level = 1,
                    ProductTypeId = x.ProductTypeId,
                }).ToListAsync();
        }
    }
}
