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

        public MaterialService(
            IUnitOfWorkCIM unitOfWork,
            IMaterialRepository materialRepository
            )
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(MaterialModel model)
        {
            /*var dbModel = new Material
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
            };*/
            var dbModel = MapperHelper.AsModel(model, new Material());
            _materialRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(MaterialModel model)
        {
            var dbModel = _materialRepository.Where(x => x.Id == model.Id).First();
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.IsDelete = false;
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _materialRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
        }
        public async Task<PagingModel<MaterialModel>> List(int page, int howmany)
        {
            var dbModel = await _materialRepository.List(page, howmany);
            var output = new List<MaterialModel>();
            foreach (var item in dbModel)
            {
                output.Add(MapperHelper.AsModel(item, new MaterialModel()));
            }

            return new PagingModel<MaterialModel>
            {
                HowMany = howmany,
                Data = output
            };
        }
        public async Task<MaterialModel> Get(int id)
        {
            var dbModel = await _materialRepository.Get(id);
            return MapperHelper.AsModel(dbModel, new MaterialModel());
        }
    }
}
