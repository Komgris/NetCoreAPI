using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using CIM.Model;
using CIM.DAL.Utility;

namespace CIM.DAL.Implements
{
    public class RouteRepository : Repository<Route, object>, IRouteRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public RouteRepository(cim_3m_1Context context, IDirectSqlRepository directSqlRepository, IConfiguration configuration ) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<RouteListModel>> List(int page, int howmany, string keyword,bool isActive,int? processTypeId)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howmany},
                                            { "@page", page},
                                             { "@processtype", processTypeId},
                                            { "@is_active", isActive},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListRoute", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<RouteListModel>(), totalCount, page, howmany);
            });
        }
    }
}
