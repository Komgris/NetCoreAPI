using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class MachineRepository : Repository<Machine>, IMachineRepository
    {
        public MachineRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {
        }

        public async Task<List<MachineTagsModel>> GetMachineTags()
        {
            return await Task.Run(() =>
            {
                var query = _entities.Machine;
                var output = query.Select(
                                            x => new MachineTagsModel(x.Id, x.Name, x.StatusTag, x.CounterInTag, x.CounterOutTag, x.CounterResetTag))
                                .ToList();
                return output;
            });
        }
    }
}
