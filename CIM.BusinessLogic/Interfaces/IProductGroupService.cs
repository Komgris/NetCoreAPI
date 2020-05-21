using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IProductGroupService : IBaseService
    {
        Task<List<RouteProductGroupModel>> ListRouteByProductGroup(int productGroupId);
        Task Create(ProductGroupModel data);
        Task<PagingModel<ProductGroupModel>> List(string keyword, int page, int howmany);
        Task<ProductGroupModel> Get(int id);
        Task Update(ProductGroupModel data);
        Task InsertMappingRouteProductGroup(List<RouteProductGroupModel> data);

    }
}
