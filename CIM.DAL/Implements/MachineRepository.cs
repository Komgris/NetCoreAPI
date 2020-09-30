using CIM.DAL.Interfaces;
using CIM.DAL.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class MachineRepository : Repository<Machine, MachineModel>, IMachineRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public MachineRepository(cim_3m_1Context context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }
        public async Task<List<RouteMachineModel>> ListMachineByRoute(int routeId)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@route_id", routeId}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMachineByRoute", parameterList);

                return (dt.ToModel<RouteMachineModel>());
            });
        }

        public async Task<List<MachineTagsModel>> GetMachineTags()
        {
            return await Task.Run(() =>
            {
                var query = _entities.Machine;
                var output = query.Where(x=>x.IsActive)
                                .Select(
                                            x => new MachineTagsModel(x.Id, x.Name, x.StatusTag, x.CounterInTag, x.CounterOutTag, x.CounterResetTag))
                                .ToList();
                return output;
            });
        }
    }
}
