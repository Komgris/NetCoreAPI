using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class RouteMachineRepository : Repository<RouteMachine, object>, IRouteMachineRepository
    {
        public RouteMachineRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {

        }
    }
}
