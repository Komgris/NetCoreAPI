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
        Task<PagingModel<ProductGroupModel>> List(string keyword, int page, int howMany, bool isActive, int? processTypeId);
        Task<ProductGroupModel> Get(int id);
        Task Update(ProductGroupModel data);
        Task Delete(int id);
        Task InsertMappingRouteProductGroup(List<RouteProductGroupModel> data);

    }
}
