using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IBomService : IBaseService
    {
        Task<PagingModel<BomModel>> List(string keyword, int page, int howmany, bool isActive);
        Task<List<BomMaterialModel>> ListBomMapping(int bomId);
        Task InsertMapping(List<BomMaterialModel> data);
        Task Create(BomModel data);
        Task Update(BomModel data);
        Task Delete(int id);
        BomModel Get(int id);
    }
}
