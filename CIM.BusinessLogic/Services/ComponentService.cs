using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIM.Domain.Models;

namespace CIM.BusinessLogic.Services
{
    public class ComponentService : BaseService, IComponentService
    {
        private IResponseCacheService _responseCacheService;
        private IComponentRepository _componentRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public ComponentService(
        IResponseCacheService responseCacheService,
        IUnitOfWorkCIM unitOfWork,
        IComponentRepository componentRepository
    )
        {
            _responseCacheService = responseCacheService;
            _componentRepository = componentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ComponentModel>> GetComponentByMachine(int machineId)
        {
            var output = new List<ComponentModel>();
            var db = await _componentRepository.Where(x => x.IsActive.Value && x.IsDelete == false && x.MachineId == machineId).ToListAsync();
            foreach (var model in db)
            {
                output.Add(MapperHelper.AsModel(model, new ComponentModel()));
            }
            return output;
        }

        public async Task<PagingModel<ComponentModel>> List(string keyword, int page, int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            var dbModel = await _componentRepository.Where(x => x.IsActive.Value && x.IsDelete == false &
                string.IsNullOrEmpty(keyword) ? true : (x.Name.Contains(keyword)))
                .Select(
                    x => new ComponentModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        MachineId = x.MachineId,
                        TypeId = x.TypeId,
                        IsActive = x.IsActive,
                        IsDelete = x.IsDelete,
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        UpdatedAt = x.UpdatedAt,
                        UpdateBy = x.UpdateBy
                    }).ToListAsync();

            int totalCount = dbModel.Count();
            dbModel = dbModel.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec).ToList();
            return ToPagingModel(dbModel, totalCount, page, howmany);
        }

        public async Task Create(ComponentModel data)
        {
            var db_model = MapperHelper.AsModel(data, new Component());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _componentRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(ComponentModel data)
        {
            var db_model = MapperHelper.AsModel(data, new Component());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdateBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _componentRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ComponentModel> Get(int id)
        {
            return await _componentRepository.All().Select(
                        x => new ComponentModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            MachineId = x.MachineId,
                            TypeId = x.TypeId,
                            IsActive = x.IsActive,
                            IsDelete = x.IsDelete,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                        }).FirstOrDefaultAsync(x => x.Id == id && x.IsActive.Value && x.IsDelete == false);
        }
    }
}
