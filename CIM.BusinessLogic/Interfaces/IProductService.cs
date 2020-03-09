using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IProductService : IBaseService
    {
        Task<PagingModel<ProductModel>> Paging(int page, int howmany);

        Task BulkEdit(List<ProductModel> model);

        Task Delete(int id);

        Task<List<ProductModel>> Create(List<ProductModel> model);

        List<ProductModel> Get();
    }
}
