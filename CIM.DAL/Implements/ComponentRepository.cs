using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class ComponentRepository : Repository<Component>, IComponentRepository
    {
        public ComponentRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {
        }

    }
}
