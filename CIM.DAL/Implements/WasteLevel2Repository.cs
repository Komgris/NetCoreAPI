using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class WasteLevel2Repository : Repository<WasteLevel2, WasteLevel2Model>, IWasteLevel2Repository
    {
        public WasteLevel2Repository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }

        public async Task<IList<WasteDictionaryModel>> ListAsDictionary()
        {
            return await _dbset.Where(x => x.IsActive == true && x.IsDelete == false)
                .Select(x => new WasteDictionaryModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    Level = 2,
                    ParentId = x.WasteLevel1Id,
                }).ToListAsync();
        }
    }
}
