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

        public async Task<PagingModel<BomModel>> List(string keyword, int page, int howmany,bool isActive)
        {
            var output = await _bomRepository.ListBom(page, howmany, keyword, isActive);
            return output;
        }

        public async Task<List<BomMaterialModel>> ListBomMapping(int bomId)
        {
            var output = await _bomRepository.ListMaterialByBom(bomId);
            return output;
        }

        public async Task InsertMapping(List<BomMaterialModel> data)
        {
            DeleteMapping(data[0].BomId);
            foreach (var model in data)
            {
                if (model.MaterialId != 0)
                {
                    var db_model = MapperHelper.AsModel(model, new BomMaterial());
                    db_model.CreatedAt = DateTime.Now;
                    db_model.CreatedBy = CurrentUser.UserId;
                    _bomMaterialRepository.Add(db_model);
                }
            }
            await _unitOfWork.CommitAsync();
        }

        public void DeleteMapping(int bomId)
        {
            var list = _bomMaterialRepository.Where(x => x.BomId == bomId);
            foreach (var model in list)
            {
                _bomMaterialRepository.Delete(model);
            }
        }

        public async Task Create(BomModel data)
        {
            var db_model = MapperHelper.AsModel(data, new BomTemp());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _bomRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(BomModel data)
        {
            var db_model = MapperHelper.AsModel(data, new BomTemp());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _bomRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

        public BomModel Get(int id)
        {
            var db_model = _bomRepository.Where(x => x.Id == id ).FirstOrDefault();
            return MapperHelper.AsModel(db_model, new BomModel());
        }

        public async Task Delete(int id)
        {
            var existingItem = _bomRepository.Where(x => x.Id == id).ToList().FirstOrDefault();
            existingItem.IsActive = false;
            existingItem.IsDelete = true;
            await _unitOfWork.CommitAsync();
        }
    }
}
