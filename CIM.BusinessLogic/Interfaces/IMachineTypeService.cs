using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMachineTypeService : IBaseService
    {
        Task Create(MachineTypeModel data);
        Task<PagingModel<MachineTypeModel>> List(string keyword, int page, int howmany, bool? isActive);
        Task<MachineTypeModel> Get(int id);
        Task Update(MachineTypeModel data);
    }
}
