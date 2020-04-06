using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IProductService : IBaseService
    {

        Task Update(ProductModel model);

        Task Delete(int id);

        Task<ProductModel> Create(ProductModel model);

        Task<PagingModel<ProductModel>> List(string keyword, int page, int howmany);

        Task<ProductModel> Get(int id);
    }
}
