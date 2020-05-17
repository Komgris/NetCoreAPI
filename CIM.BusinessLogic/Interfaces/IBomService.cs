using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IBomService : IBaseService
    {
        Task<PagingModel<BomModel>> List(string keyword, int page, int howmany);
        Task<List<BomMaterialModel>> ListBomMapping(int bomId);
    }
}
