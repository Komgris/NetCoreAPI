using CIM.DAL.Interfaces;
using CIM.DAL.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class ProductGroupRepository : Repository<ProductGroup, object>, IProductGroupRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public ProductGroupRepository(cim_3m_1Context context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<ProductGroupModel>> List(int page, int howmany, string keyword, bool isActive, int? processTypeId)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howmany},
                                            {"@page", page},
                                            {"@is_active", isActive},
                                            {"@processtypeid", processTypeId}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListProductGroup", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<ProductGroupModel>(), totalCount, page, howmany);
            });
        }

        public async Task<List<RouteProductGroupModel>> ListRouteByProductGroup(int productGroupId)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@productGroup_Id", productGroupId},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListRouteProductGroup", parameterList);

                return (dt.ToModel<RouteProductGroupModel>());
            });
        }
    }
}
