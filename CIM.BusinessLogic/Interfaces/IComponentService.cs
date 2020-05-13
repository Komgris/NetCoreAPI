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
        Task<ComponentModel> Get(int id);
        Task Update(ComponentModel data);
        Task Create(ComponentModel data);
        Task InsertMappingMachineComponent(MappingMachineComponent data);
    }
}
