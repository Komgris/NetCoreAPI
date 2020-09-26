using CIM.DAL.Interfaces;
using CIM.DAL.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class ComponentRepository : Repository<Component, ComponentModel>, IComponentRepository
    {

        private IDirectSqlRepository _directSqlRepository;
        public ComponentRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration ) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }
        public async Task<PagingModel<ComponentModel>> ListComponent(int page, int howMany, string keyword, bool isActive)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howMany},
                                            { "@page", page},
                                            { "@is_active", isActive}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListComponent", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<ComponentModel>(), totalCount, page, howMany);
            });
        }

        public async Task<List<ComponentModel>> ListComponentByMachine(int machineId)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@machine_id", machineId},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListComponentByMachine", parameterList);

                return (dt.ToModel<ComponentModel>());
            });
        }
    }
}
