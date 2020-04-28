using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<PagingModel<ProductModel>> Paging(string keyword, int page, int howmany);
        Task<List<ProductModel>> Get();
        Task<IDictionary<int, ProductDictionaryModel>> ListAsDictionary(IList<MaterialDictionaryModel> productBOM);
    }
}
