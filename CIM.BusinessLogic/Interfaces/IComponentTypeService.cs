using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IComponentTypeService : IBaseService
    {
        Task<List<ComponentTypeModel>> GetComponentTypesByMachineType(int machineTypeId);
        Task<PagingModel<ComponentTypeModel>> List(string keyword, int page, int howmany);
        Task InsertByMachineId(MappingMachineTypeComponentTypeModel<List<ComponentTypeModel>> data);
        Task Create(ComponentTypeModel data);
        Task Update(ComponentTypeModel data);
    }
}
