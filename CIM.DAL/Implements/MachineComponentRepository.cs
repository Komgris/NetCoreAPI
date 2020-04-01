using CIM.DAL.Interfaces;
using CIM.Domain.Models;

namespace CIM.DAL.Implements
{
    public class MachineComponentRepository : Repository<MachineComponent>, IMachineComponentRepository
    {
        public MachineComponentRepository(cim_dbContext context) : base(context)
        {
        }

    }
}
