using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<PagingModel<ProductModel>> Paging(int page, int howmany);
        List<ProductModel> Get();
        void InsertProduct(List<ProductModel> import);
        void DeleteProduct(string id);
    }
}
