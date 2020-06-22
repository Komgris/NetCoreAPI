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
    public class BomService : BaseService, IBomService
    {
        private IResponseCacheService _responseCacheService;
        private IBomRepository _bomRepository;
        private IBomMaterialRepository _bomMaterialRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public BomService(
        IResponseCacheService responseCacheService,
        IBomMaterialRepository bomMaterialRepository,
        IUnitOfWorkCIM unitOfWork,
        IBomRepository bomRepository
    )
        {
            _responseCacheService = responseCacheService;
            _bomMaterialRepository = bomMaterialRepository;
            _bomRepository = bomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagingModel<MaterialGroupModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            var output = await _bomRepository.ListBom(page, howMany, keyword, isActive);
            return output;
        }

        public async Task<List<MaterialGroupMaterialModel>> ListBomMapping(int bomId)
        {
            var output = await _bomRepository.ListMaterialByBom(bomId);
            output.ForEach(x => x.ImagePath = ImagePath);
            return output;
        }

        public async Task InsertMapping(List<MaterialGroupMaterialModel> data)
        {
            await DeleteMapping(data[0].BomId);
            foreach (var model in data)
            {
                if (model.MaterialId != 0)
                {
                    var db_model = MapperHelper.AsModel(model, new MaterialGroupMaterial());
                    db_model.MaterialGroupId = model.BomId;
                    db_model.CreatedAt = DateTime.Now;
                    db_model.CreatedBy = CurrentUser.UserId;
                    _bomMaterialRepository.Add(db_model);
                }
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteMapping(int bomId)
        {
            var list = await _bomMaterialRepository.WhereAsync(x => x.MaterialGroupId == bomId);
            foreach (var model in list)
            {
                _bomMaterialRepository.Delete(model);
            }
        }

        public async Task Create(MaterialGroupModel data)
        {
            var db_model = MapperHelper.AsModel(data, new MaterialGroup());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _bomRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(MaterialGroupModel data)
        {
            var db_model = MapperHelper.AsModel(data, new MaterialGroup());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _bomRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<MaterialGroupModel> Get(int id)
        {
            var db_model = await _bomRepository.Where(x => x.Id == id ).FirstOrDefaultAsync();
            return MapperHelper.AsModel(db_model, new MaterialGroupModel());
        }

        public async Task Delete(int id)
        {
            var existingItem = await _bomRepository.Where(x => x.Id == id).FirstOrDefaultAsync();
            existingItem.IsActive = false;
            existingItem.IsDelete = true;
            await _unitOfWork.CommitAsync();
        }
    }
}
