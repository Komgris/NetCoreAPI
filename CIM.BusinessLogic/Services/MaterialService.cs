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
    public class MaterialService : BaseService, IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private IProductMaterialRepository _productMaterialRepository;

        public MaterialService(
            IUnitOfWorkCIM unitOfWork,
            IMaterialRepository materialRepository,
            IProductMaterialRepository productMaterialRepository
            )
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
            _productMaterialRepository = productMaterialRepository;
        }

        public async Task<MaterialModel> Create(MaterialModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Material());
            _materialRepository.Add(dbModel);
            dbModel.IsActive = true;
            dbModel.IsDelete = false;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new MaterialModel());
        }

        public async Task<MaterialModel> Update(MaterialModel model)
        {
            var dbModel = await _materialRepository.FirstOrDefaultAsync(x => x.Id == model.Id);
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _materialRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
            return MapperHelper.AsModel(dbModel, new MaterialModel());
        }

        public async Task<PagingModel<MaterialModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            var output = await _materialRepository.List(keyword, page, howMany, isActive);
            output.Data.ForEach(x => x.ImagePath = ImagePath);
            return output;
        }

        public async Task<MaterialModel> Get(int id)
        {
            var dbModel = await _materialRepository.FirstOrDefaultAsync(x => x.Id == id);
            return MapperHelper.AsModel(dbModel, new MaterialModel());
        }

        public async Task<List<ProductMaterialModel>> ListMaterialByProduct(int productId)
        {
            var output = await _productMaterialRepository.ListMaterialByProduct(productId);
            output.ForEach(x => x.ImagePath = ImagePath);
            return output;
        }

        public async Task InsertByProduct(List<ProductMaterialModel> data)
        {
            DeleteMapping(data[0].ProductId);
            foreach (var model in data) 
            {
                if (model.MaterialId != 0)
                {
                    var db_model = MapperHelper.AsModel(model, new ProductMaterial());
                    db_model.CreatedAt = DateTime.Now;
                    db_model.CreatedBy = CurrentUser.UserId;
                    _productMaterialRepository.Add(db_model);
                }
            }
            await _unitOfWork.CommitAsync();
        }

        public void DeleteMapping(int productId)
        {
            var list = _productMaterialRepository.Where(x => x.ProductId == productId);
            foreach (var model in list)
            {
                _productMaterialRepository.Delete(model);
            }
        }

    }
}
