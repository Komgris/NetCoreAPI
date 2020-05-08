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
        private IUnitOfWorkCIM _unitOfWork;

        public MachineService(
            IUnitOfWorkCIM unitOfWork,
            IMachineRepository machineRepository,
            IResponseCacheService responseCacheService
            )
        {
            _machineRepository = machineRepository;
            _unitOfWork = unitOfWork;
            _responseCacheService = responseCacheService;
        }

        public List<MachineCacheModel> ListCached()
        {
            return new List<MachineCacheModel>();
        }

        public async Task Create(MachineModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Machine());
            _machineRepository.Add(dbModel);
            dbModel.CreatedBy = CurrentUser.UserId;
            dbModel.CreatedAt = DateTime.Now;
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
                        }).FirstOrDefaultAsync(x => x.Id == id && x.IsActive && x.IsDelete == false);
        }

        public async Task<PagingModel<MachineListModel>> List(string keyword, int page, int howmany)
        {
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            //to do optimize
            var dbModel = await _machineRepository.Where(x => x.IsActive && x.IsDelete == false &
                string.IsNullOrEmpty(keyword) ? true : (x.Name.Contains(keyword) || x.Status.Name.Contains(keyword) || x.MachineType.Name.Contains(keyword)))
                .Select(
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
                    }).ToListAsync();

            int total = dbModel.Count();
            dbModel = dbModel.OrderBy(s => s.Id).Skip(skipRec).Take(takeRec).ToList();

            var output = new List<MachineListModel>();
            foreach (var item in dbModel)
                output.Add(MapperHelper.AsModel(item, new MachineListModel()));

            return new PagingModel<MachineListModel>
            {
                HowMany = total,
                Data = output
            };
        }

        public async Task Update(MachineModel model)
        {
            var dbModel = await _machineRepository.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive && x.IsDelete == false);
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

        public async Task BulkCacheMachines(string productionPlanId, int routeId, Dictionary<int, MachineModel> machineList)
        {
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
                cachedMachine.ProductionPlanId = productionPlanId;
                await SetCached(machine.Key, cachedMachine);


            }

        }

        public async Task<List<MachineModel>> GetMachineByRoute(int routeId)
        {
            var output = await _machineRepository.ListMachineByRoute(routeId);
            return output;
        }
    }
}
