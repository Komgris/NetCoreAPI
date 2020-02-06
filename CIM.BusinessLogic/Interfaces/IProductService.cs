using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    interface IProductService
    {
        Task<PagingModel<ProductModel>> Paging(int page, int howmany);
    }
}
