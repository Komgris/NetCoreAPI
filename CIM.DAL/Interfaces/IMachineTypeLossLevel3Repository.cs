using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IMachineTypeLossLevel3Repository : IRepository<MachineTypeLossLevel3, object>
    {
        Task<PagingModel<MachineTypeLossLevel3ListModel>> List(int? machineTypeId, int? lossLevel3Id, int page, int howmany);
    }
}
