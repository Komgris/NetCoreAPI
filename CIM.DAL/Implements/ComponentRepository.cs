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
    public class ComponentRepository : Repository<Component>, IComponentRepository
    {

        private IDirectSqlRepository _directSqlRepository;
        public ComponentRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration ) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }
        public async Task<PagingModel<ComponentModel>> ListComponent(int page, int howmany, string keyword)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howmany},
                                            { "@page", page}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListComponent", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<ComponentModel>(), totalCount, page, howmany);
            });
        }

    }
}
