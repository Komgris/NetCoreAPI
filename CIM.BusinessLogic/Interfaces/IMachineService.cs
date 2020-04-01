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
        Task<PagingModel<MachineListModel>> List(string keyword, int page, int howmany);
        Task Create(MachineModel model);
        Task Update(MachineModel model);
        Task<MachineListModel> Get(int id);
    }
}
