using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IProductGroupRepository : IRepository<ProductGroup>
    {
        Task<List<RouteProductGroup>> ListRouteByProductGroup(int productGroupId);
    }
}
