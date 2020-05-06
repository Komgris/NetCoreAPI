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

        public Task InsertByMachineId(MappingMachineTypeComponentTypeModel<List<ComponentTypeModel>> data)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingModel<ComponentTypeModel>> List(string keyword, int page, int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            var dbModel = await _componentTypeRepository.Where(x => x.IsActive && x.IsDelete == false &
                string.IsNullOrEmpty(keyword) ? true : (x.Name.Contains(keyword)))
                .Select(
                    x => new ComponentTypeModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        IsDelete = x.IsDelete,
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        UpdatedAt = x.UpdatedAt,
                        UpdatedBy = x.UpdatedBy
                    }).ToListAsync();

            int totalCount = dbModel.Count();
            dbModel = dbModel.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec).ToList();
            return ToPagingModel(dbModel, totalCount, page, howmany);
        }

        public async Task Update(ComponentTypeModel data)
        {
            var db_model = MapperHelper.AsModel(data, new ComponentType());
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.IsDelete = false;
            _componentTypeRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
        }
    }
}
