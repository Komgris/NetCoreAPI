using System;
using System.Collections.Generic;
using System.Text;

using CIM.Model;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMachineTypeLossLevel3Service : IBaseService
    {
        Task<PagingModel<MachineTypeLossLevel3ListModel>> List(int? machineTypeId, int? lossLevel3Id, int page, int howmany);
    }
}
