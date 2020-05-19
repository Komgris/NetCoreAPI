using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CIM.BusinessLogic.Services
{
    public class ActiveProductionPlanService : BaseService, IActiveProductionPlanService
    {
        private IResponseCacheService _responseCacheService;
        private IMasterDataService _masterDataService;
        private IDirectSqlRepository _directSqlRepository;
        private IMachineService _machineService;
        private IProductionPlanRepository _productionPlanRepository;
        private IRecordManufacturingLossRepository _recordManufacturingLossRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public ActiveProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IDirectSqlRepository directSqlRepository,
            IMachineService machineService,
            IProductionPlanRepository productionPlanRepository,
            IRecordManufacturingLossRepository recordManufacturingLossRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _directSqlRepository = directSqlRepository;
            _machineService = machineService;
            _productionPlanRepository = productionPlanRepository;
            _recordManufacturingLossRepository = recordManufacturingLossRepository;
            _unitOfWork = unitOfWork;
        }

        public string GetKey(string productionPLanId)
        {
            return $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPLanId}";
        }

        public async Task<ActiveProductionPlanModel> GetCached(string id)
        {
            var key = GetKey(id);
            return await _responseCacheService.GetAsTypeAsync<ActiveProductionPlanModel>(key);
        }

        public async Task SetCached(ActiveProductionPlanModel model)
        {
            await _responseCacheService.SetAsync(GetKey(model.ProductionPlanId), model);
        }

        public async Task RemoveCached(string id)
        {
            var key = GetKey(id);
            await _responseCacheService.SetAsync(key, null);
        }

        /// <summary>
        /// Check if RouteId has value and is valid
        /// Get from cache, if cache is null create new object
        /// Get Db object to change StatusId
        /// Create ActiveProcesses with RouteId as key
        /// Create MachineList in ActiveProcesses
        /// BulkCacheMachines
        /// Store data to cache
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="routeId"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public async Task<ActiveProductionPlanModel> Start(string planId, int routeId, int? target)
        {

            ActiveProductionPlanModel output = null;
            //validation
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@target", target }
            };
            var isvalidatePass = _directSqlRepository.ExecuteFunction<bool>("dbo.fn_validation_plan_start", paramsList);
            if (isvalidatePass)
            {
                paramsList.Add("@user", CurrentUser.UserId);
                var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_start", paramsList);

                //Transaction success
                if (affect > 0)
                {
                    var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == planId);
                    var masterData = await _masterDataService.GetData();
                    var activeProductionPlan = (await GetCached(planId)) ?? new ActiveProductionPlanModel
                    {
                        ProductionPlanId = planId,
                    };

                    activeProductionPlan.ActiveProcesses[routeId] = new ActiveProcessModel
                    {
                        ProductionPlanId = planId,
                        ProductId = dbModel.ProductId,
                        Status = Constans.PRODUCTION_PLAN_STATUS.Production,
                        Route = new ActiveRouteModel
                        {
                            Id = routeId,
                            MachineList = masterData.Routes[routeId].MachineList.ToDictionary(x => x.Key, x => new ActiveMachineModel
                            {
                                ComponentList = x.Value.ComponentList.ToDictionary(x => x.Id, x => x),
                                Id = x.Key,
                                ProductionPlanId = planId,
                                StatusId = x.Value.StatusId,
                                RouteIds = x.Value.RouteList,
                            }),
                        }
                    };
                    var activeMachines = await _machineService.BulkCacheMachines(planId, routeId, activeProductionPlan.ActiveProcesses[routeId].Route.MachineList);
                    var notExistingStoppedMachineRecordIds = await _recordManufacturingLossRepository.GetNotExistingStoppedMachineRecord(activeMachines);
                    foreach (var machineId in notExistingStoppedMachineRecordIds)
                    {
                        var machine = activeMachines[machineId];
                        _recordManufacturingLossRepository.Add(new RecordManufacturingLoss
                        {
                            CreatedBy = machine.UserId,
                            StartedAt = machine.StartedAt,
                            IsAuto = true,
                            LossLevel3Id = Constans.DEFAULT_LOSS_LV3,
                            MachineId = machineId,
                            ProductionPlanId = planId,
                            RouteId = routeId,
                        });
                    }
                    await SetCached(activeProductionPlan);

                    output = activeProductionPlan;
                }
            }
            await _unitOfWork.CommitAsync();
            return output;

        }

        /// <summary>
        /// Change Production Plan status
        /// 
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public async Task<ActiveProductionPlanModel> Finish(string planId, int routeId)
        {
            ActiveProductionPlanModel output = null;
            var paramsList = new Dictionary<string, object>() {
                    {"@planid", planId },
                    {"@routeid", routeId }
                };

            var isvalidatePass = _directSqlRepository.ExecuteFunction<bool>("dbo.fn_validation_plan_finish", paramsList);
            if (isvalidatePass)
            {
                paramsList.Add("@user", CurrentUser.UserId);
                var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_finish", paramsList);

                //Transaction success
                if (affect > 0)
                {
                    var activeProductionPlan = await GetCached(planId);
                    if (activeProductionPlan != null)
                    {
                        var activeProcess = activeProductionPlan.ActiveProcesses[routeId];
                        activeProductionPlan.ActiveProcesses[routeId].Status = Constans.PRODUCTION_PLAN_STATUS.Finished;
                        var isPlanActive = activeProductionPlan.ActiveProcesses.Count(x => x.Value.Status != Constans.PRODUCTION_PLAN_STATUS.Finished) > 0;
                        foreach (var machine in activeProcess.Route.MachineList)
                        {
                            machine.Value.RouteIds.Remove(routeId);
                            if (!isPlanActive)
                            {
                                machine.Value.ProductionPlanId = null;
                            }
                            await _machineService.SetCached(machine.Key, machine.Value);
                        }
                        if (isPlanActive)
                        {
                            await SetCached(activeProductionPlan);
                        } else
                        {
                            await RemoveCached(activeProductionPlan.ProductionPlanId);
                            activeProductionPlan.Status = Constans.PRODUCTION_PLAN_STATUS.Finished;
                        }
                    }
                    output = activeProductionPlan;
                }
            }

            return output;
        }

        public async Task<ActiveProductionPlanModel> Pause(string planId, int routeId, int lossLevel3Id)
        {
            ActiveProductionPlanModel output = null;

            //validation
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };
            var isvalidatePass = _directSqlRepository.ExecuteFunction<bool>("dbo.fn_validation_plan_pause", paramsList);

            if (isvalidatePass)
            {
                paramsList.Add("@user", CurrentUser.UserId);
                paramsList.Add("@losslv3", lossLevel3Id);
                var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_pause", paramsList);

                //Transaction success
                if (affect > 0)
                {
                    var activeProductionPlan = await GetCached(planId);
                    activeProductionPlan.ActiveProcesses[routeId].Status = Constans.PRODUCTION_PLAN_STATUS.Pause;
                    output = activeProductionPlan;
                    await SetCached(activeProductionPlan);
                }
            }
            return output;
        }

        public async Task<ActiveProductionPlanModel> Resume(string planId, int routeId)
        {

            ActiveProductionPlanModel output = null;
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            var isvalidatePass = _directSqlRepository.ExecuteFunction<bool>("dbo.fn_validation_plan_resume", paramsList);
            if (isvalidatePass)
            {
                paramsList.Add("@user", CurrentUser.UserId);
                var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_resume", paramsList);

                //Transaction success
                if (affect > 0)
                {
                    var activeProductionPlan = await GetCached(planId);
                    activeProductionPlan.ActiveProcesses[routeId].Status = Constans.PRODUCTION_PLAN_STATUS.Production;
                    output = activeProductionPlan;
                    await SetCached(activeProductionPlan);
                }
            }
            return output;
        }
    }
}
