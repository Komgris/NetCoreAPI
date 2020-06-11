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
    public class MachineRepository : Repository<Machine>, IMachineRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public MachineRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<MachineListModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howMany},
                                            {"@page", page},
                                            {"@is_active", isActive},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMachine", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<MachineListModel>(), totalCount, page, howMany);
            });
        }
        public async Task<List<RouteMachineModel>> ListMachineByRoute(int routeId,string imagePath)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@route_id", routeId},
                                            {"@imagepath", imagePath }
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
