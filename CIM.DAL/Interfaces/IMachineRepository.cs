using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IMachineRepository : IRepository<Machine>
    {
        Task<PagingModel<MachineListModel>> List(string keyword, int page, int howMany, bool isActive);
        Task<List<MachineTagsModel>> GetMachineTags();
        Task<List<RouteMachineModel>> ListMachineByRoute(int routeId, string imagePath);
    }
}
