using CIM.DAL.Interfaces;
using CIM.Domain.Models;

namespace CIM.DAL.Implements
{
    public class RouteMachineRepository : Repository<RouteMachine>, IRouteMachineRepository
    {
        public RouteMachineRepository(cim_dbContext context) : base(context)
        {

        }
    }
}
