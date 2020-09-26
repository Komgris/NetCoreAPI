using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IProductGroupRepository : IRepository<ProductGroup, object>
    {
        Task<List<RouteProductGroupModel>> ListRouteByProductGroup(int productGroupId);
        Task<PagingModel<ProductGroupModel>> List(int page, int howmany, string keyword, bool isActive, int? processTypeId);
    }
}
