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
using Newtonsoft.Json;

namespace CIM.BusinessLogic.Services
{
    public class MachineService : BaseService, IMachineService
    {
        IResponseCacheService _responseCacheService;
        IMachineRepository _machineRepository;
        IRouteMachineRepository _routeMachineRepository;
        IRecordMachineStatusRepository _recordMachineStatusRepository;
        IDirectSqlRepository _directSqlRepository;
        IUnitOfWorkCIM _unitOfWork;
        IMasterDataService _masterDataService;
        string systemInterfaceInfoKey = "systemInfoInter_3M";

        public MachineService(
            IUnitOfWorkCIM unitOfWork,
            IMachineRepository machineRepository,
            IMasterDataService masterDataService,
            IResponseCacheService responseCacheService,
            IRouteMachineRepository routeMachineRepository,
            IRecordMachineStatusRepository recordMachineStatusRepository,
            IDirectSqlRepository directSqlRepository
            )
        {
            _directSqlRepository = directSqlRepository;
            _machineRepository = machineRepository;
            _unitOfWork = unitOfWork;
            _responseCacheService = responseCacheService;
            _routeMachineRepository = routeMachineRepository;
            _recordMachineStatusRepository = recordMachineStatusRepository;
            _masterDataService = masterDataService;
        }

        public List<MachineCacheModel> ListCached()
        {
            return new List<MachineCacheModel>();
        }

        public async Task Create(MachineListModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new Machine(), new[] { "Status" });
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
                            MachineTypeId = x.MachineTypeId,
                            //Type = x.MachineType.Name,
                            StatusTag = x.StatusTag,
                            CounterInTag = x.Speed,
                            CounterOutTag = x.CounterOutTag,
                            CounterResetTag = x.CounterResetTag,
                            IsActive = x.IsActive,
                            IsDelete = x.IsDelete,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                            UpdatedBy = x.UpdatedBy
                        }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<MachineModel>> List()
        {
            var output = await _machineRepository.List("sp_ListMachine", new Dictionary<string, object>());
            return output;
        }

        public async Task Update(MachineListModel model)
        {
            var dbModel = await _machineRepository.FirstOrDefaultAsync(x => x.Id == model.Id);
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
        public async Task<ActiveMachine3MModel> GetCached3M(int id)
        {
            return _responseCacheService.GetActiveMachine(id);
        }

        public async Task SetCached3M(ActiveMachine3MModel model)
        {
            await _responseCacheService.SetActiveMachine(model);
        }

        public async Task SetCached(int id, ActiveMachineModel model)
        {
            await _responseCacheService.SetAsync(CachedKey(id), model);
        }


        public async Task RemoveCached(int id, ActiveMachineModel model)
        {
            await _responseCacheService.RemoveAsync(CachedKey(id));
        }
        public async Task<ActiveMachine3MModel> BulkCacheMachines3M(string productionPlanId, int machineId)
        {
            var output = new ActiveMachine3MModel();
            //var seq = 0;
            //foreach (var machine in machineList)
            //{
            //seq++;
            var key = CachedKey(machineId);
            var cachedMachine = await GetCached3M(machineId);
            if (cachedMachine == null)
            {
                cachedMachine = new ActiveMachine3MModel
                {
                    Id = machineId,
                    //Sequence = seq,
                    UserId = CurrentUser.UserId,
                    StartedAt = DateTime.Now,
                    StatusId = Constans.MACHINE_STATUS.NA,
                };
            }
            if (cachedMachine.StatusId == Constans.MACHINE_STATUS.NA)
            {
                var dbModel = await _recordMachineStatusRepository.Where(x => x.MachineId == machineId).OrderBy(x => x.CreatedAt).FirstOrDefaultAsync();
                if (dbModel != null)
                    cachedMachine.StatusId = dbModel.MachineStatusId;
            }
            cachedMachine.ProductionPlanId = productionPlanId;

            await SetCached3M(cachedMachine);
            //output.Add(cachedMachine);
            //}
            return output;

        }

        public async Task<Dictionary<int, ActiveMachineModel>> BulkCacheMachines(string productionPlanId, int routeId, Dictionary<int, ActiveMachineModel> machineList)
        {
            var output = new List<ActiveMachineModel>();
            var seq = 0;
            foreach (var machine in machineList)
            {
                seq++;
                var key = CachedKey(machine.Key);
                var cachedMachine = await GetCached(machine.Key);
                if (cachedMachine == null)
                {
                    cachedMachine = new ActiveMachineModel
                    {
                        Id = machine.Key,
                        Sequence = seq,
                        RouteIds = new List<int> { routeId },
                        UserId = CurrentUser.UserId,
                        StartedAt = DateTime.Now,
                        StatusId = Constans.MACHINE_STATUS.NA,
                    };
                }
                else
                {
                    if (cachedMachine.RouteIds == null)
                    {
                        cachedMachine.RouteIds = new List<int>();
                    }

                    //if (!cachedMachine.RouteIds.Contains(routeId))
                    //    cachedMachine.RouteIds.Add(routeId);

                    cachedMachine.Sequence = seq;
                }

                if (cachedMachine.StatusId == Constans.MACHINE_STATUS.NA /*|| cachedMachine.StatusId == Constans.MACHINE_STATUS.Unknown*/)
                {
                    var dbModel = await _recordMachineStatusRepository.Where(x => x.MachineId == machine.Key).OrderBy(x => x.CreatedAt).FirstOrDefaultAsync();
                    if (dbModel != null)
                        cachedMachine.StatusId = dbModel.MachineStatusId;
                }
                cachedMachine.ProductionPlanId = productionPlanId;

                await SetCached(machine.Key, cachedMachine);
                output.Add(cachedMachine);
            }
            return output.ToDictionary(x => x.Id, x => x);

        }

        public async Task InsertMappingRouteMachine(List<RouteMachineModel> data)
        {
            await DeleteMapping(data[0].RouteId);
            int sequence = 1;
            foreach (var model in data)
            {
                if (model.MachineId != 0)
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
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<RouteMachineModel>> GetMachineByRoute(int routeId)
        {
            var output = await _machineRepository.ListMachineByRoute(routeId);
            return output;
        }
        public async Task DeleteMapping(int routeid)
        {
            var list = await _routeMachineRepository.WhereAsync(x => x.RouteId == routeid);
            foreach (var model in list)
            {
                _routeMachineRepository.Delete(model);
            }
        }

        #region HW interface

        //public async Task<List<MachineTagsModel>> GetMachineTags()
        //{
        //    return await _machineRepository.GetMachineTags();
        //}

        public async Task SetListMachinesResetCounter(List<int> machines, bool isCounting)
        {
            if (machines!.Count > 0)
            {
                var model = await SystemInterfaceInfo();
                foreach (var mcid in machines)
                {
                    if (!model.ListMachineIdsResetCounter.ContainsKey(mcid))
                    {
                        model.ListMachineIdsResetCounter.Add(mcid, isCounting);
                    }
                    else
                    {
                        model.ListMachineIdsResetCounter[mcid] = isCounting;
                    }
                }
                await _responseCacheService.SetAsync(systemInterfaceInfoKey, model);
            }
        }

        public async Task ForceInitialTags()
        {
            var model = await SystemInterfaceInfo();
            model.HasTagChanged = true;
            await _responseCacheService.SetAsync(systemInterfaceInfoKey, model);
        }

        public async Task<SystemInterfaceModel> GetSystemInterfaceInfo()
        {
            var result = await SystemInterfaceInfo();           
            await _responseCacheService.SetAsync(systemInterfaceInfoKey, null);
            return result;
        }

        public async Task InitialMachineCache()
        {
            var masterdata = await _masterDataService.GetData();
            foreach (var mc in masterdata.Machines)
            {
                var mccache = GetCached(mc.Key).Result;
                if (mccache == null)
                {
                    await SetCached3M(new ActiveMachine3MModel
                    {
                        Id = mc.Key,
                        Image = mc.Value.Image,
                        UserId = CurrentUser.UserId,
                        StatusId = 2,
                        StartedAt = DateTime.Now
                    });
                }
                else
                {
                    mccache.Image = mc.Value.Image;
                    await SetCached(mc.Key, mccache);
                }
            }
        }

        private async Task<SystemInterfaceModel> SystemInterfaceInfo()
        {
            var cache = await _responseCacheService.GetAsTypeAsync<SystemInterfaceModel>(systemInterfaceInfoKey);
            if (cache is null)
            {
                cache = new SystemInterfaceModel();
            }
            cache.ProductionInfo = _responseCacheService.GetProductionInfo();
            return cache;
        }

        public async Task SetMachinesResetCounter3M(int machineId, bool isCounting)
        {
            
                var model = await SystemInterfaceInfo();
                    if (!model.ListMachineIdsResetCounter.ContainsKey(machineId))
                    {
                        model.ListMachineIdsResetCounter.Add(machineId, isCounting);
                    }
                    else
                    {
                        model.ListMachineIdsResetCounter[machineId] = isCounting;
                    }
                await _responseCacheService.SetAsync(systemInterfaceInfoKey, model);           
        }
        public MachineInfoModel GetProductInfoData(string planId)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@plan_id", planId }
            };
            var dt =  JsonConvert.SerializeObject(_directSqlRepository.ExecuteSPWithQuery("sp_GetProductInfo", paramsList));
            var list = JsonConvert.DeserializeObject<List<MachineInfoModel>>(dt);
            return list.FirstOrDefault();
        }
        public async Task SetProductInfoCache(ProductionInfoModel info)
        {
            await _responseCacheService.SetProductionInfo(info);
        }

        public async Task<ProductionInfoModel> GetProductionInfoCache()
        {
            return  _responseCacheService.GetProductionInfo();
        }
        #endregion
    }
}
