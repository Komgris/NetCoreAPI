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
    public class SystemParameterService : BaseService, ISystemParameterService
    {
        private IUnitOfWorkCIM _unitOfWork;
        private ISystemParameterRepository _systemParameterRepository;

        public SystemParameterService(
            IUnitOfWorkCIM unitOfWork,
            ISystemParameterRepository systemParameterRepository
            )
        {
            _unitOfWork = unitOfWork;
            _systemParameterRepository = systemParameterRepository;
        }

        public async Task Create(SystemParameterModel data)
        {
            var db_model = MapperHelper.AsModel(data, new SystemParameter());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            _systemParameterRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(SystemParameterModel data)
        {
            var db_model = MapperHelper.AsModel(data, new SystemParameter());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _systemParameterRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var db_model = _systemParameterRepository.Where(x => x.Id == id).ToList().FirstOrDefault();
            db_model.IsActive = false;
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            await _unitOfWork.CommitAsync();
        }

        public async Task<SystemParameterModel> Get(int id)
        {
            return await _systemParameterRepository.All().Select(
                x => new SystemParameterModel
                {
                    Id = x.Id,
                    Parameter = x.Parameter,
                    Value = x.Value,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy
                }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagingModel<SystemParameterModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            var output = await _systemParameterRepository.ListAsPaging("sp_ListSystemParameter", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    {"@page", page},
                    {"@is_active", isActive}
                }, page, howMany);
            return output;
        }
    }
}
