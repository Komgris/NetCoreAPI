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
    public class MachineTypeService : BaseService, IMachineTypeService
    {
        private IResponseCacheService _responseCacheService;
        private IUnitOfWorkCIM _unitOfWork;
        private IMachineTypeRepository _machineTypeRepository;

        public MachineTypeService(
            IResponseCacheService responseCacheService,
            IUnitOfWorkCIM unitOfWork,
            IMachineTypeRepository machineTypeRepository
            )
        {
            _responseCacheService = responseCacheService;
            _unitOfWork = unitOfWork;
            _machineTypeRepository = machineTypeRepository;
        }

        public async Task Create(MachineTypeModel data)
        {
            var db_model = MapperHelper.AsModel(data, new MachineType());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _machineTypeRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(MachineTypeModel data)
        {
            var db_model = MapperHelper.AsModel(data, new MachineType());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _machineTypeRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<PagingModel<MachineTypeModel>> List(string keyword, int page, int howmany, bool isActive)
        {
            var output = await _machineTypeRepository.List(keyword, page, howmany, isActive);
            output.Data.ForEach(x => x.ImagePath = ImagePath);
            return output;
        }

        public async Task<MachineTypeModel> Get(int id)
        {
            return await _machineTypeRepository.All().Select(
                        x => new MachineTypeModel
                        {
                            Id = x.Id,
                            Image = x.Image,
                            Name = x.Name,
                            HasOee = x.HasOee,
                            IsActive = x.IsActive,
                            IsDelete = x.IsDelete,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                            UpdatedBy = x.UpdatedBy
                        }).FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
