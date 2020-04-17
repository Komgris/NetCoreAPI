using CIM.DAL.Interfaces;
using CIM.Domain.Models;

namespace CIM.DAL.Implements
{
    public class RouteProductGroupRepository : Repository<RouteProductGroup>, IRouteProductGroupRepository
    {
        public RouteProductGroupRepository(cim_dbContext context) : base(context)
        {

        }
    }
}
