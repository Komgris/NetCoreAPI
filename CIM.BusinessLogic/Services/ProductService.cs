using CIM.BusinessLogic.Interfaces;
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
        private IUserAppTokenRepository _userAppTokenRepository;
        private ICipherService _cipherService;
        private IProductRepository _productRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public ProductService(
            IUserAppTokenRepository userAppTokenRepository,
            ICipherService cipherService,
            IProductRepository productRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _userAppTokenRepository = userAppTokenRepository;
            _cipherService = cipherService;
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
                var existingItem = _productRepository.Where(x => x.Id == plan.Id).FirstOrDefault();
                if(existingItem != null)
                {
                    var dbModel = new Product
                    {
                        Code = plan.Code,
                        Description = plan.Code,
                        BriteItemUpcItem = plan.BriteItemUpcItem,
                        ProductFamilyId = plan.ProductFamilyId,
                        ProductGroupId = plan.ProductGroupId,
                        ProductTypeId = plan.ProductTypeId,
                        PackingMedium = plan.PackingMedium,
                        IgWeight = plan.IgWeight,
                        PmWeight = plan.PmWeight,
                        WeightUom = plan.WeightUom,
                        IsActive = true,
                        IsDelete = false,
                        UpdatedAt = DateTime.Now,
                    };
                    _productRepository.Edit(dbModel);
                }
            }
            await _unitOfWork.CommitAsync();           
        }
        public async void Delete(int Id)
        {
            var existingItem = _productRepository.Where(x => x.Id == Id).ToList().FirstOrDefault();
            _productRepository.Delete(existingItem);
            await _unitOfWork.CommitAsync();
        }
        public async void Create(List<ProductModel> model)
        {
            foreach (var plan in model)
            {
                var dbModel = new Product
                {
                    Code = plan.Code,
                    Description = plan.Code,
                    BriteItemUpcItem = plan.BriteItemUpcItem,
                    ProductFamilyId = plan.ProductFamilyId,
                    ProductGroupId = plan.ProductGroupId,
                    ProductTypeId = plan.ProductTypeId,
                    PackingMedium = plan.PackingMedium,
                    IgWeight = plan.IgWeight,
                    PmWeight = plan.PmWeight,
                    WeightUom = plan.WeightUom,
                    IsActive = true,
                    IsDelete = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,                   
                };
                _productRepository.Add(dbModel); 
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
