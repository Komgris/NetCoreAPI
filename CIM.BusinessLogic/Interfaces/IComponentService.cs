using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IComponentService
    {
        Task<List<ComponentModel>> GetComponentByMachine(int machineId);
        Task<PagingModel<ComponentModel>> List(string keyword, int page, int howmany);
    }
}
