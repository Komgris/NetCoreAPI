﻿using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Services {
    public class ActiveProductionPlanService : BaseService, IActiveProductionPlanService {
        IDashboardService _dashboardService;
        private IResponseCacheService _responseCacheService;
        private IMasterDataService _masterDataService;
        private IDirectSqlRepository _directSqlRepository;
        private IMachineService _machineService;
        private IProductionPlanRepository _productionPlanRepository;
        private IRecordManufacturingLossRepository _recordManufacturingLossRepository;
        private IRecordMachineStatusRepository _recordMachineStatusRepository;
        private IRecordProductionPlanOutputRepository _recordProductionPlanOutputRepository;
        private IMachineOperatorRepository _machineOperatorRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private IMachineRepository _machineRepository;
        public IConfiguration _config;
        private IRecordManufacturingLossService _recordManufacturingLossService;
        private IRecordActiveProductionPlanRepository _activeproductionPlanRepository;

        public ActiveProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IDirectSqlRepository directSqlRepository,
            IMachineService machineService,
            IProductionPlanRepository productionPlanRepository,
            IRecordManufacturingLossRepository recordManufacturingLossRepository,
            IRecordMachineStatusRepository recordMachineStatusRepository,
            IRecordProductionPlanOutputRepository recordProductionPlanOutputRepository,
            IMachineOperatorRepository machineOperatorRepository,
            IUnitOfWorkCIM unitOfWork,
            IDashboardService dashboardService,
            IMachineRepository machineRepository,
            IConfiguration config,
            IRecordManufacturingLossService recordManufacturingLossService,
            IRecordActiveProductionPlanRepository activeproductionPlanRepository
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
            _machineOperatorRepository = machineOperatorRepository;
            _unitOfWork = unitOfWork;
            _machineRepository = machineRepository;
            _config = config;
            _recordManufacturingLossService = recordManufacturingLossService;
            _activeproductionPlanRepository = activeproductionPlanRepository;
            _dashboardService = dashboardService;
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
            await _responseCacheService.RemoveAsync(key);
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
            var now = DateTime.Now;

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
                        Image = x.Value.Image
                    });
                    var activeMachines = await _machineService.BulkCacheMachines(planId, routeId, routeMachines);

                    //record operators
                    foreach (var machine in activeMachines)
                    {
                        var machineOperator = await _machineOperatorRepository.FirstOrDefaultAsync(x => x.MachineId == machine.Key && x.PlanId == planId);
                        if (machineOperator == null)
                        {
                            var machineTeamCount = await _machineOperatorRepository.ExecuteProcedure<int>("[dbo].[sp_CountMachineEmployee]", new Dictionary<string, object> {
                                 {"@machine_id", machine.Key }
                            });
                            _machineOperatorRepository.Add(new MachineOperators
                            {
                                LastUpdatedAt = now,
                                MachineId = machine.Key,
                                PlanId = planId,
                                LastUpdatedBy = CurrentUser.UserId,
                                OperatorCount = machineTeamCount,
                                OperatorPlan = machineTeamCount
                            });
                        }

                    }

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

                    //update another route are use the same machines
                    foreach (var mcmultiRoute in activeMachines.Where(a=> a.Value.RouteIds.Count > 1) )
                    {
                        foreach (var r in mcmultiRoute.Value.RouteIds)
                        {
                            if (r != routeId)
                                activeProductionPlan.ActiveProcesses[r].Route.MachineList[mcmultiRoute.Key].RouteIds.Add(routeId);
                        }
                    }

                    var runningMachineIds = activeMachines.Where(x => x.Value.StatusId == Constans.MACHINE_STATUS.Idle).Select(x => x.Key).ToArray();
                    foreach (var machineId in runningMachineIds)
                    {
                        _recordMachineStatusRepository.Add(new RecordMachineStatus {
                            CreatedAt = now,
                            MachineId = machineId,
                            MachineStatusId = Constans.MACHINE_STATUS.Running,
                        });
                        UpdateMachineStatus(machineId, Constans.MACHINE_STATUS.Running);
                    }

                    //start counting output
                    var mcfirstStart = activeMachines.Where(x => x.Value.RouteIds.Count == 1).Select(o=>o.Key).ToList();
                    await _machineService.SetListMachinesResetCounter(mcfirstStart, true);

                    //generate -> BoardcastData
                    activeProductionPlan.ActiveProcesses[routeId].BoardcastData = await _dashboardService.GenerateBoardcast(DataTypeGroup.All, planId, routeId);
                    await SetCached(activeProductionPlan);
                    output = activeProductionPlan;

                    //get last plan with same route
                    var preplan = await _activeproductionPlanRepository.Where(x => x.RouteId == routeId && now.Date == x.Start.Date ).OrderByDescending(x => x.Start).Take(2).ToListAsync();
                    var preproductId = preplan.Count == 2 ? await _productionPlanRepository.Where(x => x.PlanId == preplan[1].ProductionPlanPlanId).Select(o => o.ProductId).FirstOrDefaultAsync()
                                                            : dbModel.ProductId;

                    //record time loss on process ramp-up #139
                    var rampupModel = new RecordManufacturingLossModel()
                    {
                        ProductionPlanId = planId,
                        RouteId = routeId,
                        LossLevelId = preproductId != dbModel.ProductId ? 
                                                                      _config.GetValue<int>("DefaultChangeOverlv3Id")
                                                                    : _config.GetValue<int>("DefaultProcessDrivenlv3Id"),
                        IsAuto = true
                    };
                    foreach (var machine in activeMachines)
                    {
                        //Is first start for machine
                        if (machine.Value.RouteIds.Count == 1)
                        {
                            //activeProductionPlan.ActiveProcesses[routeId].Route.MachineList[machine.Key].IsReady = true;
                            rampupModel.MachineId = machine.Key;
                            await _recordManufacturingLossService.Create(rampupModel);
                        }
                    }
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

            //get message for descripe validation result
            var message = _directSqlRepository.ExecuteFunction<string>("dbo.fn_validation_plan_finish", paramsList);
            if (message != ""){
                throw (new Exception(message));
            }
            else
            {
                paramsList.Add("@user", CurrentUser.UserId);
                var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_finish", paramsList);

                //Transaction success
                if (affect > 0)
                {
                    var activeProductionPlan = await GetCached(planId);
                    if (activeProductionPlan != null)
                    {
                        var mcliststopCounting = new List<int>();
                        var activeProcess = activeProductionPlan.ActiveProcesses[routeId];
                        activeProductionPlan.ActiveProcesses[routeId].Status = Constans.PRODUCTION_PLAN_STATUS.Finished;
                        var isPlanActive = activeProductionPlan.ActiveProcesses.Count(x => x.Value.Status != Constans.PRODUCTION_PLAN_STATUS.Finished) > 0;

                        //update another route are use the same machines
                        foreach (var mcmultiRoute in activeProcess.Route.MachineList.Where(a => a.Value.RouteIds.Count > 1))
                        {
                            foreach (var r in mcmultiRoute.Value.RouteIds)
                            {
                                if (r != routeId)
                                    activeProductionPlan.ActiveProcesses[r].Route.MachineList[mcmultiRoute.Key].RouteIds.Remove(routeId);
                            }
                        }

                        foreach (var machine in activeProcess.Route.MachineList)
                        {
                            machine.Value.RouteIds.Remove(routeId);
                            if (machine.Value.RouteIds.Count == 0) mcliststopCounting.Add(machine.Key);
                            if (!isPlanActive) machine.Value.ProductionPlanId = null;
                            await _machineService.SetCached(machine.Key, machine.Value);
                        }


                        //stop counting output
                        await _machineService.SetListMachinesResetCounter(mcliststopCounting, false);
                        if (isPlanActive)
                        {
                            await SetCached(activeProductionPlan);
                        }
                        else
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
            var now = DateTime.Now;
            var activeRoute = new List<int>();
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
                    StatusId = statusId,
                    StartedAt = DateTime.Now
                };
            }

            //if machine is apart of production plan
            if (!string.IsNullOrEmpty(cachedMachine.ProductionPlanId) && cachedMachine.RouteIds != null)
            {
                output = await GetCached(cachedMachine.ProductionPlanId);
                if (output != null)
                {
                    foreach (var routeId in cachedMachine.RouteIds.Distinct())
                    {
                        if (output.ActiveProcesses.ContainsKey(routeId))
                        {
                            UpdateMachineStatus(machineId, statusId);
                            activeRoute.Add(routeId);

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

            if (cachedMachine.StatusId != recordMachineStatusId)
            {
                cachedMachine.StatusId = recordMachineStatusId;
                var paramsList = new Dictionary<string, object>() {
                    {"@planid", cachedMachine.ProductionPlanId },
                    {"@mcid", machineId },
                    {"@statusid", recordMachineStatusId }
                };
                _directSqlRepository.ExecuteSPNonQuery("sp_Process_Machine_Status", paramsList);
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
                                && x.IsAuto == true 
                                && !(x.LossLevel3Id == _config.GetValue<int>("DefaultChangeOverlv3Id") || x.LossLevel3Id == _config.GetValue<int>("DefaultProcessDrivenlv3Id"))
                ); //update only isAuto = true && not rampUp

            var now = DateTime.Now;

            foreach (var dbModel in losses)
            {
                var alert = activeProductionPlan.ActiveProcesses[routeId].Alerts.FirstOrDefault(x => x.Id == Guid.Parse(dbModel.Guid));
                if(alert != null)
                    alert.EndAt = now;
                dbModel.EndAt = now;
                dbModel.EndBy = CurrentUser.UserId;
                dbModel.Timespan = Convert.ToInt64((now - dbModel.StartedAt).TotalSeconds);
                dbModel.IsBreakdown = dbModel.Timespan >= 600;//10 minute
                if (dbModel.Timespan < 60 && dbModel.IsAuto)
                {
                    dbModel.LossLevel3Id = _config.GetValue<int>("DefaultSpeedLosslv3Id");
                    var sploss = activeProductionPlan.ActiveProcesses[routeId].Alerts.FirstOrDefault(x => x.Id == Guid.Parse(dbModel.Guid));
                    //handle case alert is removed from redis
                    if (sploss != null)
                    {
                        sploss.LossLevel3Id = dbModel.LossLevel3Id;
                        sploss.StatusId = (int)Constans.AlertStatus.Edited;
                    }
                }
                _recordManufacturingLossRepository.Edit(dbModel);
            }
            return activeProductionPlan;
        }

        private async Task<ActiveProductionPlanModel> HandleMachineStop(int machineId, int statusId, ActiveProductionPlanModel activeProductionPlan, int routeId, bool isAuto)
        {
            var now = DateTime.Now;

            var cachedMachine = await _machineService.GetCached(machineId);
            var firstRoute = cachedMachine.RouteIds.First();
            if (cachedMachine.RouteIds.Count() > 0)
            {
                // create new alert and record only on first route of machine
                var isFirstRoute = firstRoute == routeId;
                if (!activeProductionPlan.ActiveProcesses[routeId].Route.MachineList[machineId].IsReady) // has unclosed record inside
                {
                    AlertModel alert;

                    if (isFirstRoute)
                    {
                        alert = new AlertModel
                        {
                            StatusId = (int)Constans.AlertStatus.New,
                            ItemStatusId = statusId,
                            CreatedAt = now,
                            Id = Guid.NewGuid(),
                            LossLevel3Id = _config.GetValue<int>("DefaultLosslv3Id"),
                            ItemId = machineId,
                            ItemType = (int)Constans.AlertType.MACHINE,
                            RouteId = routeId
                        };

                        _recordManufacturingLossRepository.Add(new RecordManufacturingLoss
                        {
                            CreatedBy = CurrentUser.UserId,
                            Guid = alert.Id.ToString(),
                            IsAuto = isAuto,
                            LossLevel3Id = _config.GetValue<int>("DefaultLosslv3Id"),
                            MachineId = machineId,
                            ProductionPlanId = activeProductionPlan.ProductionPlanId,
                            StartedAt = now,
                            RouteId = routeId
                        });

                    }
                    // else reuse existing alert of first route and don't insert new other record
                    else
                    {
                        alert = activeProductionPlan.ActiveProcesses[firstRoute].Alerts
                            .Where(x =>
                                x.ItemId == machineId &&
                                x.StatusId == (int)Constans.AlertStatus.New &&
                                x.EndAt == null).OrderByDescending(x => x.CreatedAt).First();

                        alert.RouteId = routeId;
                    }

                    activeProductionPlan.ActiveProcesses[routeId].Alerts.Add(alert);

                }

            }
            return activeProductionPlan;
        }

        public async Task<int[]> ListMachineReady(string productionPlanId)
        {
            return await _recordManufacturingLossRepository.ListMachineReady(new Dictionary<string, object> {
                {"@plan_id", productionPlanId }
            });
        }
       
        public async Task<int[]> ListMachineLossRecording(string productionPlanId)
        {
            return await _recordManufacturingLossRepository.ListMachineLossRecording(new Dictionary<string, object> {
                {"@plan_id", productionPlanId }
            });
        }
        public async Task<int[]> ListMachineLossAutoRecording(string productionPlanId)
        {
            return await _recordManufacturingLossRepository.ListMachineLossRecording(new Dictionary<string, object> {
                {"@plan_id", productionPlanId },{"@isauto", true }
            });
        }

        public async Task<List<ActiveProductionPlanModel>> UpdateMachineOutput(List<MachineProduceCounterModel> listData, int hour)
        {
            var machineList = new List<ActiveMachineModel>();
            var exemachineIds = new List<int>();
            var activeProductionPlanList = new List<ActiveProductionPlanModel>();
            var now = DateTime.Now;
            foreach (var item in listData)
            {
                var cachedMachine = await _machineService.GetCached(item.MachineId);
                if (cachedMachine?.ProductionPlanId != null)
                {
                    //inActiveproductionplan stuck in cache
                    if (GetCached(cachedMachine.ProductionPlanId).Result == null)
                    {
                        await RemoveCached(cachedMachine.ProductionPlanId);
                        continue;
                    }

                    //via store
                    var paramsList = new Dictionary<string, object>() {
                            {"@planid", cachedMachine.ProductionPlanId },
                            {"@mcid", item.MachineId },
                            {"@user", CurrentUser.UserId},
                            {"@hr", hour},
                            {"@tIn",  item.CounterIn},
                            {"@tOut", item.CounterOut}
                        };


                    //db execute
                    var dbOutput = await _recordProductionPlanOutputRepository
                                                            .Where(x => x.MachineId == cachedMachine.Id && x.ProductionPlanId == cachedMachine.ProductionPlanId)
                                                            .OrderByDescending(x => x.CreatedAt)
                                                            .Take(2).ToListAsync();

                    if (dbOutput.Count > 0 && (dbOutput[0].TotalOut >= item.CounterOut && dbOutput[0].TotalIn >= item.CounterIn))
                    {
                        //do nothing
                    }
                    else if (cachedMachine != null)
                    {
                        if (dbOutput.Count == 0 || cachedMachine.RecordProductionPlanOutput == null || dbOutput[0].Hour != hour)
                        {
                            if (dbOutput.Count > 0)
                            {
                                paramsList.Add("@cIn", item.CounterIn - dbOutput[0].TotalIn);
                                paramsList.Add("@cOut", item.CounterOut - dbOutput[0].TotalOut);

                                File.AppendAllText(@"C:\log\CounterAdd_logging"
                                    , $"{DateTime.Now.ToString("dd-MM-yy HH:mm:ss")} >> Plan:{cachedMachine.ProductionPlanId} | Machine:{item.MachineId} | Hour:{dbOutput[0].Hour}<->{hour} | RecordsCnt:{dbOutput.Count} \r\n");
                            }
                            else//close ramp-up records and start to operating time #139
                            {
                                paramsList.Add("@cIn", item.CounterIn);
                                paramsList.Add("@cOut", item.CounterOut);

                                foreach (var routeid in cachedMachine.RouteIds)
                                {
                                    var activeplan = await GetCached(cachedMachine.ProductionPlanId);
                                    if (activeplan?.ActiveProcesses[routeid]?.Route.MachineList != null)
                                    {
                                        var dmodel = await _recordManufacturingLossRepository
                                            .WhereAsync(x => activeplan.ActiveProcesses[routeid].Route.MachineList.Keys.Contains((int)x.MachineId)
                                            && x.IsAuto == true && x.EndAt.HasValue == false);

                                        foreach (var activemc in activeplan.ActiveProcesses[routeid].Route.MachineList)
                                        {
                                            //close ramp-up records for front machine in the same route #139
                                            if (!exemachineIds.Contains(activemc.Key) && activemc.Value.IsReady && dmodel.Where(x => x.MachineId == activemc.Key).Any())
                                            {
                                                var model = new RecordManufacturingLossModel()
                                                {
                                                    ProductionPlanId = cachedMachine.ProductionPlanId,
                                                    MachineId = activemc.Key,
                                                    RouteId = routeid,
                                                };
                                                await _recordManufacturingLossService.End(model);
                                                exemachineIds.Add(activemc.Key);
                                            }
                                            if (activemc.Key == item.MachineId) break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //update
                            paramsList.Add("@id", dbOutput[0].Id);
                            paramsList.Add("@cIn", item.CounterIn - (dbOutput.Count > 1 ? dbOutput[1].TotalIn : 0));
                            paramsList.Add("@cOut", item.CounterOut - (dbOutput.Count > 1 ? dbOutput[1].TotalOut : 0));
                        }
                        _directSqlRepository.ExecuteSPNonQuery("sp_Process_Production_Counter", paramsList);
                    }
                   
                    //set cache
                    cachedMachine.RecordProductionPlanOutput = new RecordProductionPlanOutputModel { Hour = hour, Input = item.CounterIn, Output = item.CounterOut };
                    machineList.Add(cachedMachine);
                    await _machineService.SetCached(item.MachineId, cachedMachine);
                }
            }

            var activeProductionPlanIds = machineList.Select(x => x.ProductionPlanId).Distinct().ToList();
            foreach (var item in activeProductionPlanIds)
            {
                var activeProductionPlan = await GetCached(item);
                activeProductionPlan.Machines = machineList.Where(x => x.ProductionPlanId == activeProductionPlan.ProductionPlanId)?.ToDictionary(x => x.Id, x => x);
                activeProductionPlanList.Add(activeProductionPlan);
                await SetCached(activeProductionPlan);

            }
            await _unitOfWork.CommitAsync();
            return activeProductionPlanList;
        }

        public async Task<ActiveProductionPlanModel> AdditionalMachineOutput(string planId, int? machineId, int? routeId, int amount, int? hour,string remark)
        {
            var now = DateTime.Now;
            if(machineId != null)
            {
                var cachedMachine = await _machineService.GetCached((int)machineId);
                if (cachedMachine?.ProductionPlanId != null)
                {
                    //inActiveproductionplan stuck in cache
                    if (GetCached(cachedMachine.ProductionPlanId).Result == null)
                    {
                        await RemoveCached(cachedMachine.ProductionPlanId);
                    }
                }
            }

            //store proc.
            var paramsList = new Dictionary<string, object>() {
                        {"@planid", planId },
                        {"@routeid", routeId },
                        {"@mcid", machineId },
                        {"@user", CurrentUser.UserId},
                        {"@hr", hour},
                        {"@Add",  amount} ,
                        {"@remark",  remark}
            };

            //db execute
            _directSqlRepository.ExecuteSPNonQuery("sp_Process_Production_Counter_Additional", paramsList);

            var activeProductionPlan = await GetCached(planId);
            return activeProductionPlan;
        }

        private void UpdateMachineStatus(int machineId,int statusId)
        {
            var mc = _machineRepository.Where(x => x.Id == machineId).FirstOrDefault();
            if(mc != null)
            {
                mc.UpdatedAt = DateTime.Now;
                mc.UpdatedBy = CurrentUser.UserId;
                mc.StatusId = statusId;
                _machineRepository.Edit(mc);
            }
        }
    }
}
