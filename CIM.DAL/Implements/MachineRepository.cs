using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class MachineRepository : Repository<Machine>, IMachineRepository
    {
        public MachineRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {
        }

    }
}
