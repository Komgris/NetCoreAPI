using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMachineOperatorService : IBaseService
    {

        Task Create(MachineOperatorModel model);
        Task Update(MachineOperatorModel model);
        Task Delete(int id);

    }
}
