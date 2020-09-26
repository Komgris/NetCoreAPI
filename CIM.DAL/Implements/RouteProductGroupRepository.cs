using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class RouteProductGroupRepository : Repository<RouteProductGroup, object>, IRouteProductGroupRepository
    {
        public RouteProductGroupRepository(cim_3m_1Context context, IConfiguration configuration ) : base(context, configuration)
        {

        }
    }
}
