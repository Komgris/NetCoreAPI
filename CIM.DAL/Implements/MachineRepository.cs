using CIM.DAL.Interfaces;
using CIM.DAL.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class MachineRepository : Repository<Machine>, IMachineRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public MachineRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<List<MachineModel>> ListMachineByRoute(int routeId)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@route_id", routeId},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMachineByRoute", parameterList);

                return (dt.ToModel<MachineModel>());
            });
        }

    }
}
