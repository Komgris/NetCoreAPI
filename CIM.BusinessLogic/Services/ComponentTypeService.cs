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
    public class ComponentTypeService : BaseService, IComponentTypeService
    {
        private IResponseCacheService _responseCacheService;
        private IComponentTypeRepository _componentTypeRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private IMachineTypeComponentTypeRepository _machineTypeComponentTypeRepository;

        public ComponentTypeService(
            IResponseCacheService responseCacheService,
            IUnitOfWorkCIM unitOfWork,
            IComponentTypeRepository componentTypeRepository,
            IMachineTypeComponentTypeRepository machineTypeComponentTypeRepository
            )
        {
            _responseCacheService = responseCacheService;
            _unitOfWork = unitOfWork;
            _componentTypeRepository = componentTypeRepository;
            _machineTypeComponentTypeRepository = machineTypeComponentTypeRepository;
        }

        public async Task Create(ComponentTypeModel data)
        {
            var db_model = MapperHelper.AsModel(data, new ComponentType());
            db_model.CreatedAt = DateTime.Now;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _componentTypeRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<ComponentTypeModel>> GetComponentTypesByMachineType(int machineTypeId)
        {
            var output = await _componentTypeRepository.ListComponentTypeByMachineType(machineTypeId);
            return output;
        }

        public async Task InsertByMachineId(MappingMachineTypeComponentTypeModel<List<ComponentTypeModel>> data)
        {
            await DeleteMapping(data.MachineId);
            foreach (var model in data.Component)
            {
                var db = new MachineTypeComponentType();
                db.MachineTypeId = data.MachineId;
                db.ComponentTypeId = model.Id;
                db.CreatedAt = DateTime.Now;
                db.CreatedBy = CurrentUser.UserId;
                _machineTypeComponentTypeRepository.Add(db);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<PagingModel<ComponentTypeModel>> List(string keyword, int page, int howmany,bool isActive)
        {
            var output = await _componentTypeRepository.List(keyword, page, howmany, isActive, ImagePath);
            return output;
        }

        public async Task Update(ComponentTypeModel data)
        {
            var db_model = MapperHelper.AsModel(data, new ComponentType());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _componentTypeRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteMapping(int mc_id)
        {
            var list = _machineTypeComponentTypeRepository.Where(x => x.MachineTypeId == mc_id);
            foreach (var model in list)
            {
                _machineTypeComponentTypeRepository.Delete(model);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<ComponentTypeModel> Get(int id)
        {
            return await _componentTypeRepository.All().Select(
                        x => new ComponentTypeModel
                        {
                            Id = x.Id,
                            Image = x.Image,
                            Name = x.Name,
                            IsActive = x.IsActive,
                            IsDelete = x.IsDelete,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                            UpdatedBy = x.UpdatedBy
                        }).FirstOrDefaultAsync(x => x.Id == id );
        }
    }
}
