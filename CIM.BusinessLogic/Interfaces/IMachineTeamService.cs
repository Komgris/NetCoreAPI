using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMachineTeamService : IBaseService
    {
        Task Create(MachineTeamModel data);
        Task<PagingModel<MachineTeamModel>> List(string keyword, int page, int howMany, bool isActive);
        Task<MachineTeamModel> Get(int id);
        Task Update(MachineTeamModel data);
    }
}
