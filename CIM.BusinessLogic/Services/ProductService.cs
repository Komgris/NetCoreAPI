using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(
            IUnitOfWorkCIM unitOfWork,
            IProductRepository productRepository
            )
        {
            _productRepository = productRepository;
        }
        public Task<PagingModel<ProductModel>> Paging(int page, int howmany)
        {
            var result = _productRepository.Paging(page, howmany);
            return result;
        }
    }
}
