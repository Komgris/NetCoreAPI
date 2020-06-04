using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IBomRepository :IRepository<BomTemp>
    {
        Task<List<BomMaterialModel>> ListMaterialByBom(int bomId);
        Task<PagingModel<BomModel>> ListBom(int page, int howmany, string keyword, bool isActive);
    }
}
