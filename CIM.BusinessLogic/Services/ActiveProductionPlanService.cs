using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static CIM.Model.Constans;

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
        private IRecordMachineStatusRepository _recordMachineStatusRepository;
        private IRecordProductionPlanOutputRepository _recordProductionPlanOutputRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private IReportService _reportService;

        public ActiveProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IDirectSqlRepository directSqlRepository,
            IMachineService machineService,
            IProductionPlanRepository productionPlanRepository,
            IRecordManufacturingLossRepository recordManufacturingLossRepository,
            IRecordMachineStatusRepository recordMachineStatusRepository,
            IRecordProductionPlanOutputRepository recordProductionPlanOutputRepository,
            IUnitOfWorkCIM unitOfWork,
            IReportService reportService
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _directSqlRepository = directSqlRepository;
            _machineService = machineService;
            _productionPlanRepository = productionPlanRepository;
            _recordManufacturingLossRepository = recordManufacturingLossRepository;
            _recordMachineStatusRepository = recordMachineStatusRepository;
            _recordProductionPlanOutputRepository = recordProductionPlanOutputRepository;
            _unitOfWork = unitOfWork;
            _reportService = reportService;
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
                    var activeProductionPlan = (await GetCached(planId)) ?? new ActiveProductionPlanModel(planId);

                    var routeMachines = masterData.Routes[routeId].MachineList.ToDictionary(x => x.Key, x => new ActiveMachineModel
                    {
                        ComponentList = x.Value.ComponentList.ToDictionary(x => x.Id, x => x),
                        Id = x.Key,
                        ProductionPlanId = planId,
                        RouteIds = x.Value.RouteList,
                    });
                    var activeMachines = await _machineService.BulkCacheMachines(planId, routeId, routeMachines);

                    activeProductionPlan.ActiveProcesses[routeId] = new ActiveProcessModel
                    {
                        ProductionPlanId = planId,
                        ProductId = dbModel.ProductId,
                        Status = Constans.PRODUCTION_PLAN_STATUS.Production,
                        Route = new ActiveRouteModel
                        {
                            Id = routeId,
                            MachineList = activeMachines,
                        }
                    };
                    var notExistingStoppedMachineRecordIds = await _recordManufacturingLossRepository.GetNotExistingStoppedMachineRecord(activeMachines);
                    foreach (var machineId in notExistingStoppedMachineRecordIds)
                    {
                        activeProductionPlan = await HandleMachineStop(machineId, Constans.MACHINE_STATUS.Stop, activeProductionPlan, routeId, true);
                    }

                    var runningMachineIds = activeMachines.Where(x => x.Value.StatusId == Constans.MACHINE_STATUS.Idle).Select(x => x.Key).ToArray();
                    foreach (var machineId in runningMachineIds)
                    {
                        _recordMachineStatusRepository.Add(new RecordMachineStatus {
                            CreatedAt = DateTime.Now,
                            MachineId = machineId,
                            MachineStatusId = Constans.MACHINE_STATUS.Running,
                        });
                    }

                    //generate -> BoardcastData
                    activeProductionPlan.ActiveProcesses[routeId].BoardcastData = await _reportService.GenerateBoardcastData(BoardcastType.All, planId, routeId);
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

        public async Task<ActiveProductionPlanModel> UpdateByMachine(int machineId, int statusId, bool isAuto)
        {
            var cachedMachine = await _machineService.GetCached(machineId);
            var masterData = await _masterDataService.GetData();
            var machine = masterData.Machines[machineId];
            ActiveProductionPlanModel output = null;
            // If Production Plan doesn't start but machine just start to send status
            if (cachedMachine == null)
            {
                cachedMachine = new ActiveMachineModel
                {
                    Id = machine.Id,
                    UserId = CurrentUser.UserId,
                    StartedAt = DateTime.Now
                };
            }

            //if machine is apart of production plan
            if (!string.IsNullOrEmpty(cachedMachine.ProductionPlanId) && cachedMachine.RouteIds != null)
            {
                output = await GetCached(cachedMachine.ProductionPlanId);
                if (output != null)
                {
                    foreach (var routeId in cachedMachine.RouteIds)
                    {
                        if (output.ActiveProcesses.ContainsKey(routeId))
                        {
                            output.ActiveProcesses[routeId].Route.MachineList[machineId].StatusId = statusId;
                            output = await HandleMachineByStatus(machineId, statusId, output, routeId, isAuto);
                        }
                    }
                    await SetCached(output);
                }
            }

            //handle machine status
            var recordMachineStatusId = statusId;
            if (string.IsNullOrEmpty(cachedMachine.ProductionPlanId) && statusId == Constans.MACHINE_STATUS.Running)
            {
                recordMachineStatusId = Constans.MACHINE_STATUS.Idle;
            }
            cachedMachine.StatusId = recordMachineStatusId;
            var lastRecordMachineStatus = await _recordMachineStatusRepository.Where(x => x.MachineId == machineId)
                .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

            if (lastRecordMachineStatus == null || lastRecordMachineStatus.MachineStatusId != recordMachineStatusId)
            {
                var recordMachineStatus = new RecordMachineStatus
                {
                    CreatedAt = DateTime.Now,
                    MachineId = machineId,
                    ProductionPlanId = cachedMachine.ProductionPlanId,
                    MachineStatusId = recordMachineStatusId
                };

                _recordMachineStatusRepository.Add(recordMachineStatus);
            }

            await _unitOfWork.CommitAsync();
            await _machineService.SetCached(machineId, cachedMachine);
            return output;
        }

        private async Task<ActiveProductionPlanModel> HandleMachineByStatus(int machineId, int statusId, ActiveProductionPlanModel activeProductionPlan, int routeId, bool isAuto)
        {
            switch (statusId)
            {
                case Constans.MACHINE_STATUS.Stop: activeProductionPlan = await HandleMachineStop(machineId, statusId, activeProductionPlan, routeId, isAuto); break;
                case Constans.MACHINE_STATUS.Running: activeProductionPlan = await HandleMachineRunning(machineId, statusId, activeProductionPlan, routeId, isAuto); break;
                default: break;
            }
            return activeProductionPlan;
        }

        private async Task<ActiveProductionPlanModel> HandleMachineRunning(int machineId, int statusId, ActiveProductionPlanModel activeProductionPlan, int routeId, bool isAuto)
        {
            var losses = await _recordManufacturingLossRepository
                .WhereAsync(x => x.MachineId == machineId && x.EndAt == null && x.RouteId == routeId 
                && x.IsAuto == true); //update only isAuto = true
            var now = DateTime.Now;

            foreach (var dbModel in losses)
            {
                dbModel.EndAt = now;
                dbModel.EndBy = CurrentUser.UserId;
                dbModel.Timespan = Convert.ToInt64((now - dbModel.StartedAt).TotalSeconds);
                _recordManufacturingLossRepository.Edit(dbModel);
            }
            return activeProductionPlan;
        }

        private async Task<ActiveProductionPlanModel> HandleMachineStop(int machineId, int statusId, ActiveProductionPlanModel activeProductionPlan, int routeId, bool isAuto)
        {
            var now = DateTime.Now;
            var dbModel = await _recordManufacturingLossRepository.FirstOrDefaultAsync(x => x.MachineId.HasValue && x.MachineId.Value == machineId && x.EndAt.HasValue == false);
            if (dbModel == null)//is recording?
            {
                var alert = new AlertModel
                {
                    StatusId = (int)Constans.AlertStatus.New,
                    ItemStatusId = statusId,
                    CreatedAt = now,
                    Id = Guid.NewGuid(),
                    LossLevel3Id = Constans.DEFAULT_LOSS_LV3,
                    ItemId = machineId,
                    ItemType = (int)Constans.AlertType.MACHINE,
                    RouteId = routeId
                };
                activeProductionPlan.Alerts.Add(alert);

                _recordManufacturingLossRepository.Add(new RecordManufacturingLoss
                {
                    CreatedBy = CurrentUser.UserId,
                    Guid = alert.Id.ToString(),
                    IsAuto = isAuto,
                    LossLevel3Id = Constans.DEFAULT_LOSS_LV3,
                    MachineId = machineId,
                    ProductionPlanId = activeProductionPlan.ProductionPlanId,
                    StartedAt = now,
                    RouteId = routeId
                });
            }
            return activeProductionPlan;
        }

        public async Task<List<ActiveProductionPlanModel>> UpdateMachineOutput(List<MachineProduceCounterModel> listData, int hour)
        {
            var machineList = new List<ActiveMachineModel>();
            var activeProductionPlanList = new List<ActiveProductionPlanModel>();
            var now = DateTime.Now;
            foreach (var item in listData)
            {
                var cachedMachine = await _machineService.GetCached(item.MachineId);
                if (cachedMachine?.ProductionPlanId != null)
                {
                    var recordOutput = new RecordProductionPlanOutput();
                    var dbOutput = _recordProductionPlanOutputRepository
                                                            .Where(x => x.MachineId == cachedMachine.Id && x.ProductionPlanId == cachedMachine.ProductionPlanId)
                                                            .OrderByDescending(x => x.CreatedAt)
                                                            .Take(2).ToList();
                    if (dbOutput.Count == 0 || cachedMachine?.RecordProductionPlanOutput == null || cachedMachine?.RecordProductionPlanOutput.Hour != hour)
                    {
                        recordOutput = new RecordProductionPlanOutput
                        {
                            CounterIn = item.CounterIn,
                            CounterOut = item.CounterOut,
                            CreatedAt = now,
                            CreatedBy = CurrentUser.UserId,
                            Hour = hour,
                            MachineId = item.MachineId,
                            ProductionPlanId = cachedMachine.ProductionPlanId,
                            TotalIn = item.CounterIn,
                            TotalOut = item.CounterOut,
                        };

                        if (dbOutput.Count > 0)
                        {
                            recordOutput.CounterIn = item.CounterIn - dbOutput[0].TotalIn;
                            recordOutput.CounterOut = item.CounterOut - dbOutput[0].TotalOut;
                        }

                        //insert
                        _recordProductionPlanOutputRepository.Add(recordOutput);
                    }
                    else
                    {
                        //update
                        recordOutput = dbOutput[0];
                        recordOutput.CounterIn = item.CounterIn - (dbOutput.Count > 1? dbOutput[1].TotalIn:0);
                        recordOutput.CounterOut = item.CounterOut - (dbOutput.Count > 1 ? dbOutput[1].TotalOut : 0);
                        recordOutput.TotalIn = item.CounterIn;
                        recordOutput.TotalOut = item.CounterOut;
                        recordOutput.UpdatedAt = now;

                        _recordProductionPlanOutputRepository.Edit(recordOutput);
                    }

                    //set cache
                    cachedMachine.RecordProductionPlanOutput = new RecordProductionPlanOutputModel { Hour = hour,  Input = item.CounterIn, Output = item.CounterOut };
                    machineList.Add(cachedMachine);
                    await _machineService.SetCached(item.MachineId, cachedMachine);
                }
            }

            var activeProductionPlanIds = machineList.Select(x => x.ProductionPlanId).Distinct().ToList();
            foreach (var item in activeProductionPlanIds)
            {
                var activeProductionPlan = await GetCached(item);
                activeProductionPlan.Machines = machineList.Where(x => x.ProductionPlanId == activeProductionPlan.ProductionPlanId).ToDictionary(x => x.Id, x => x);
                activeProductionPlanList.Add(activeProductionPlan);
                await SetCached(activeProductionPlan);

            }
            await _unitOfWork.CommitAsync();
            return activeProductionPlanList;
        }
    }
}
