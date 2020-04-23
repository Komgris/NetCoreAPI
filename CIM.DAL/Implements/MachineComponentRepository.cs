using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class MachineComponentRepository : Repository<MachineComponent>, IMachineComponentRepository
    {
        public MachineComponentRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {
        }

    }
}
