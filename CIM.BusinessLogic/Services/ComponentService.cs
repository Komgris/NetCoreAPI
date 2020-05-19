﻿using CIM.BusinessLogic.Interfaces;
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
            var output = await _componentRepository.ListComponentByMachine(machineId);
            return output;
        }

        public async Task<PagingModel<ComponentModel>> List(string keyword, int page, int howmany)
        {
            var output = await _componentRepository.ListComponent(page, howmany, keyword);
            return output;
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

        public async Task InsertMappingMachineComponent(MappingMachineComponent data)
        {
            var dbModels = await _componentRepository.Where(x=> x.MachineId == data.MachineId).ToListAsync();
            foreach(var model in dbModels)
            {
                model.MachineId = null;
                _componentRepository.Edit(model);
            }

            var componentIds = data.ComponentList.Select(x => x.Id);
            var components = await _componentRepository.WhereAsync(x => componentIds.Contains(x.Id)  );

            foreach (var model in components)
            {
                model.MachineId = data.MachineId;
                _componentRepository.Edit(model);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<ComponentModel>> GetComponentNoMachineId(string keyword)
        {
            var dbModels = await _componentRepository.WhereAsync(x => x.MachineId == null);
            var components = dbModels.Where(x=> string.IsNullOrEmpty(keyword) ? true : (x.Name.Contains(keyword)))
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
                    }).ToList();
            return components;
        }
    }
}
