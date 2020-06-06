using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IBomService : IBaseService
    {
        Task<PagingModel<MaterialGroupModel>> List(string keyword, int page, int howMany, bool isActive);
        Task<List<MaterialGroupMaterialModel>> ListBomMapping(int bomId);
        Task InsertMapping(List<MaterialGroupMaterialModel> data);
        Task Create(MaterialGroupModel data);
        Task Update(MaterialGroupModel data);
        Task Delete(int id);
        Task<MaterialGroupModel> Get(int id);
    }
}
