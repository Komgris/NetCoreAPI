using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IComponentRepository : IRepository<Component>
    {
        Task<PagingModel<ComponentModel>> ListComponent(int page, int howMany, string keyword, bool isActive);
        Task<List<ComponentModel>> ListComponentByMachine(int machineId);
    }
}
