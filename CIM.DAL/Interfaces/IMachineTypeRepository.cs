using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IMachineTypeRepository : IRepository<MachineType>
    {
        Task<PagingModel<MachineTypeModel>> List(string keyword, int page, int howmany, bool isActive);
    }
}
