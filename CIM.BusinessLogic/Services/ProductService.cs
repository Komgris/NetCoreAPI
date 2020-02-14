using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ProductService: BaseService, IProductService
    {
        private IProductRepository _productRepository;
        private IUnitOfWorkCIM _unitOfWork;
        public ProductService(
            IProductRepository productRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<PagingModel<ProductModel>> Paging(int page, int howmany)
        {
            var result = _productRepository.Paging(page, howmany);
            return result;
        }

        public async void BulkEdit(List<ProductModel> model)
        {
            foreach(var plan in model)
            {
                var existingItem = _productRepository.Where(x => x.Code == plan.Code).FirstOrDefault();
                if(existingItem != null)
                {
                    var db_model = MapperHelper.AsModel(plan, existingItem);
                    _productRepository.Edit(db_model);
                }
            }
            await _unitOfWork.CommitAsync();           
        }

        public async void Delete(string code)
        {
            var existingItem = _productRepository.Where(x => x.Code == code).ToList().FirstOrDefault();
            _productRepository.Delete(existingItem);
            await _unitOfWork.CommitAsync();
        }

        public async Task Create(List<ProductModel> model)
        {
            foreach (var plan in model)
            {
                var db_model = MapperHelper.AsModel(plan, new Product());
                _productRepository.Add(db_model);               
            }
             await _unitOfWork.CommitAsync();
        }
    }
}
