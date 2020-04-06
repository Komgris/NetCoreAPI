using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ProductService: BaseService, IProductService
    {
        private readonly IResponseCacheService _responseCacheService;
        private readonly IProductRepository _productRepository;
        private IUnitOfWorkCIM _unitOfWork;
        public ProductService(
            IProductRepository productRepository,
            IUnitOfWorkCIM unitOfWork,
            IResponseCacheService responseCacheService
            )
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _responseCacheService = responseCacheService; 
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

        public async Task<ProductModel> Create(ProductModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Product());
            _productRepository.Add(dbModel);
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            dbModel.IsActive = true;
            dbModel.IsDelete = false;
            await _unitOfWork.CommitAsync();
            var response = MapperHelper.AsModel(dbModel, new ProductModel());

            return response;
        }

        public async Task<PagingModel<ProductModel>> List(string keyword, int page, int howmany)
        {
            var output = await _productRepository.Paging(keyword, page, howmany);
            return output;
        }

        public async Task<ProductModel> Get(int id)
        {
            var dbModel = await _productRepository.Where(x => x.Id == id && x.IsActive && x.IsDelete == false)
                .Select(
                        x => new ProductModel
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Description = x.Description,
                            BriteItemPerUpcitem = x.BriteItemPerUpcitem,
                            ProductFamily_Id = x.ProductFamily_Id,
                            ProductGroup_Id = x.ProductGroup_Id,
                            ProductType_Id = x.ProductType_Id,
                            PackingMedium = x.PackingMedium,
                            NetWeight = x.NetWeight,
                            Igweight = x.Igweight,
                            Pmweight = x.Pmweight,
                            WeightPerUom = x.WeightPerUom,
                            IsActive = x.IsActive,
                            IsDelete = x.IsDelete,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                            UpdatedBy = x.UpdatedBy,
                        }).FirstOrDefaultAsync();
            return MapperHelper.AsModel(dbModel, new ProductModel());
        }

        public async Task Update(ProductModel model)
        {
            var dbModel = await _productRepository.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive && x.IsDelete == false);
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _productRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
        }
    }
}
