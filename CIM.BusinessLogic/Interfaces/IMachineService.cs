using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMachineService : IBaseService
    {
        List<MachineCacheModel> ListCached();
        Task<PagingModel<MachineModel>> List(string keyword, int page, int howmany);
        Task<MachineModel> Create(MachineModel model);
        Task<MachineModel> Update(MachineModel model);
        Task<MachineModel> Get(int id);
    }
}
