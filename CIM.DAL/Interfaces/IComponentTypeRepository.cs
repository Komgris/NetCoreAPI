using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IComponentTypeRepository : IRepository<ComponentType>
    {
        Task<List<ComponentTypeModel>> ListComponentTypeByMachineType(int machineTypeId);
        Task<PagingModel<ComponentTypeModel>> List(string keyword, int page, int howMany, bool isActive, string imagePath);
    }
}
