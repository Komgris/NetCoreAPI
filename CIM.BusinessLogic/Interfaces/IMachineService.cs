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
        Task<MachineListModel> Create(MachineListModel model);
        Task<MachineListModel> Update(MachineListModel model);
        Task<MachineListModel> Get(int id);
    }
}
