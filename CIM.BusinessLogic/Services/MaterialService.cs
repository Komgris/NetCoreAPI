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
    public class MaterialService : BaseService, IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public MaterialService(
            IUnitOfWorkCIM unitOfWork,
            IMaterialRepository materialRepository
            )
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }

        public async void Create(MaterialModel model)
        {
            var dbModel = new Material
            {
                Code = model.Code,
                Description = model.Description,
                ProductCategory = model.ProductCategory,
                ICSGroup = model.ICSGroup,
                MaterialGroup = model.MaterialGroup,
                UOM = model.UOM,
                BHTPerUnit = model.BHTPerUnit,
                IsActive = true,
                IsDelete = false,
                CreatedBy = CurrentUser.UserId,
                CreatedAt = DateTime.Now,
                UpdatedBy = CurrentUser.UserId,
                UpdatedAt = DateTime.Now
            };
            _materialRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
        }

        public async void Update(MaterialModel model)
        {
            var dbModel = _materialRepository.Where(x => x.Id == model.Id).First();
            if (dbModel != null)
            {
                dbModel.Code = model.Code;
                dbModel.Description = model.Description;
                dbModel.ProductCategory = model.ProductCategory;
                dbModel.ICSGroup = model.ICSGroup;
                dbModel.MaterialGroup = model.MaterialGroup;
                dbModel.UOM = model.UOM;
                dbModel.BHTPerUnit = model.BHTPerUnit;
                dbModel.IsActive = model.IsActive;
                dbModel.IsDelete = false;
                dbModel.UpdatedBy = CurrentUser.UserId;
                dbModel.UpdatedAt = DateTime.Now;
                _materialRepository.Edit(dbModel);
                await _unitOfWork.CommitAsync();
            }
        }
        public async Task<PagingModel<MaterialModel>> List(int page, int howmany)
        {
            var result = await _materialRepository.List(page, howmany);
            return result;
        }
        public async Task<MaterialModel> Get(int id)
        {
            var dbModel = await _materialRepository.Get(id);
            return dbModel;
        }
    }
}
