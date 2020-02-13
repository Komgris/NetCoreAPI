using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IProductService
    {
        Task<PagingModel<ProductModel>> Paging(int page, int howmany);

        void BulkEdit(List<ProductModel> model);

        void Delete(int Id);

        void Create(List<ProductModel> model);
    }
}
