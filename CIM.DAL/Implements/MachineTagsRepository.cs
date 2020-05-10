using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class MachineTagsRepository : Repository<Machine>, IMachineTagsRepository
    {
        private IMachineTagsRepository _machineTagsRepository;
        public MachineTagsRepository(cim_dbContext context, IConfiguration configuration) : base(context, configuration)
        {
            //_machineTagsRepository = machineTagsRepository;
        }

        public async Task<List<MachineTagsModel>> Get()
        {
            return await Task.Run(() =>
            {
                var query = _entities.Machine;
                //List<MachineTagsModel> output = new List<MachineTagsModel>();
                var output = query.Select(
                            x => new MachineTagsModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                RunningStatus = new MachineTagsSup<bool>(x.StatusTag, false),
                                CounterIn = new MachineTagsSup<int>(x.CounterInTag, 0),
                                CounterOut = new MachineTagsSup<int>(x.CounterOutTag, 0),
                                CounterReset = new MachineTagsSup<bool>(x.CounterResetTag, false)
                            }).ToList();
                return output;
            });
        }
    }
}
