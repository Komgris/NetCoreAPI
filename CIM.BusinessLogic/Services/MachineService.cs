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
    public class MachineService : BaseService, IMachineService
    {
        private readonly IResponseCacheService _responseCacheService;
        private readonly IMachineRepository _machineRepository;
        private readonly IRouteMachineRepository _routeMachineRepository;
        private readonly IRecordMachineStatusRepository _recordMachineStatusRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public MachineService(
            IUnitOfWorkCIM unitOfWork,
            IMachineRepository machineRepository,
            IResponseCacheService responseCacheService,
            IRouteMachineRepository routeMachineRepository,
            IRecordMachineStatusRepository recordMachineStatusRepository
            )
        {
            _machineRepository = machineRepository;
            _unitOfWork = unitOfWork;
            _responseCacheService = responseCacheService;
            _routeMachineRepository = routeMachineRepository;
            _recordMachineStatusRepository = recordMachineStatusRepository;
        }

        public List<MachineCacheModel> ListCached()
        {
            return new List<MachineCacheModel>();
        }

        public async Task Create(MachineModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Machine());
            dbModel.StatusId = Constans.MACHINE_STATUS.Idle;
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
            _machineRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
        }

        public async Task<MachineListModel> Get(int id)
        {
            return await _machineRepository.All().Select(
                        x => new MachineListModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            StatusId = x.StatusId,
                            Status = x.Status.Name,
                            MachineTypeId = x.MachineTypeId,
                            Type = x.MachineType.Name,
                            IsActive = x.IsActive,
                            IsDelete = x.IsDelete,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                            UpdatedBy = x.UpdatedBy
                        }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagingModel<MachineListModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            var output = await _machineRepository.List(keyword, page, howMany, isActive);
            return output;
        }

        public async Task Update(MachineModel model)
        {
            var dbModel = await _machineRepository.FirstOrDefaultAsync(x => x.Id == model.Id );
            dbModel = MapperHelper.AsModel(model, dbModel);
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.UpdatedAt = DateTime.Now;
            _machineRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
        }

        public string CachedKey(int id)
        {
            return $"{Constans.RedisKey.MACHINE}:{id}";
        }

        public async Task<ActiveMachineModel> GetCached(int id)
        {
            return await _responseCacheService.GetAsTypeAsync<ActiveMachineModel>(CachedKey(id));
        }

        public async Task SetCached(int id, ActiveMachineModel model)
        {
            await _responseCacheService.SetAsync(CachedKey(id), model);
        }

        public async Task RemoveCached(int id, ActiveMachineModel model)
        {
            await _responseCacheService.SetAsync(CachedKey(id), model);
        }

        public async Task<Dictionary<int, ActiveMachineModel>> BulkCacheMachines(string productionPlanId, int routeId, Dictionary<int, ActiveMachineModel> machineList)
        {
            var output = new List<ActiveMachineModel>();
            foreach (var machine in machineList)
            {
                var key = CachedKey(machine.Key);
                var cachedMachine = await GetCached(machine.Key);
                if (cachedMachine == null)
                {
                    cachedMachine = new ActiveMachineModel
                    {
                        Id = machine.Key,
                        RouteIds = new List<int> { routeId },
                        UserId = CurrentUser.UserId,
                        StartedAt = DateTime.Now,
                        StatusId = Constans.MACHINE_STATUS.Unknown,
                    };
                }
                else
                {
                    if (cachedMachine.RouteIds == null)
                    {
                        cachedMachine.RouteIds = new List<int>();
                    }
                    cachedMachine.RouteIds.Add(routeId);
                }
                
                if (cachedMachine.StatusId == Constans.MACHINE_STATUS.NA || cachedMachine.StatusId == Constans.MACHINE_STATUS.Unknown)
                {
                    var dbModel = await _recordMachineStatusRepository.Where(x => x.MachineId == machine.Key).OrderBy(x => x.CreatedAt).FirstOrDefaultAsync();
                    if (dbModel != null)
                        cachedMachine.StatusId = dbModel.MachineStatusId;
                }
                cachedMachine.ProductionPlanId = productionPlanId;

                await SetCached(machine.Key, cachedMachine);
                output.Add(cachedMachine);
            }
            return output.ToDictionary( x=>x.Id, x => x);

        }

        public async Task InsertMappingRouteMachine( List<RouteMachineModel> data)
        {
            await DeleteMapping(data[0].RouteId);
            int sequence = 1;
            foreach (var model in data)
            {

                var db_model = MapperHelper.AsModel(model, new RouteMachine());
                db_model.IsActive = true;
                db_model.IsDelete = false;
                db_model.CreatedAt = DateTime.Now;
                db_model.CreatedBy = CurrentUser.UserId;
                db_model.Sequence = sequence;
                _routeMachineRepository.Add(db_model);
                sequence++;
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<MachineModel>> GetMachineByRoute(int routeId)
        {
            var output = await _machineRepository.ListMachineByRoute(routeId);
            return output;
        }
        public async Task DeleteMapping(int routeid)
        {
            var list = _routeMachineRepository.Where(x => x.RouteId == routeid);
            foreach (var model in list)
            {
                _routeMachineRepository.Delete(model);
            }
            await _unitOfWork.CommitAsync();
        }
        public async Task<List<MachineTagsModel>> GetMachineTags()
        {
            return await _machineRepository.GetMachineTags();
        }
    }
}
