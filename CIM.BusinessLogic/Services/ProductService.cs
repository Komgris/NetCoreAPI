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

        public async Task BulkEdit(List<ProductModel> model)
        {
            foreach(var plan in model)
            {
                var existingItem = _productRepository.Where(x => x.Id == plan.Id).FirstOrDefault();
                if(existingItem != null)
                {
                    var db_model = MapperHelper.AsModel(plan, existingItem);
                    _productRepository.Edit(db_model);
                }
            }
            await _unitOfWork.CommitAsync();           
        }

        public async Task Delete(int id)
        {
            var existingItem = _productRepository.Where(x => x.Id == id).ToList().FirstOrDefault();
            _productRepository.Delete(existingItem);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<ProductModel>> Create(List<ProductModel> model)
        {
            List<ProductModel> db_list = new List<ProductModel>();
            foreach (var plan in model)
            {
                var db_model = MapperHelper.AsModel(plan, new Product());              
                _productRepository.Add(db_model);
                db_list.Add(MapperHelper.AsModel(db_model, new ProductModel()));
            }
            await _unitOfWork.CommitAsync();
            return db_list;
        }
        public List<ProductModel> Get()
        {
            var db = _productRepository.All().ToList();
            List<ProductModel> productDb = new List<ProductModel>();
            foreach (var plan in db)
            {
                var db_model = MapperHelper.AsModel(plan, new ProductModel());
                productDb.Add(db_model);
            }
            return productDb;
        }
    }
}
