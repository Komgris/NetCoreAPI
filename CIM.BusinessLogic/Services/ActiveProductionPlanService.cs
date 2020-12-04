using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
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
using Newtonsoft.Json;

namespace CIM.BusinessLogic.Services
{
    public class ActiveProductionPlanService : BaseService, IActiveProductionPlanService
    {
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

        public async Task<ActiveProductionPlan3MModel> GetCached3M(string planId)
        {
            //var key = GetKey(id);
            return _responseCacheService.GetActivePlan(planId);
        }

        public async Task SetCached(ActiveProductionPlanModel model)
        {
            await _responseCacheService.SetAsync(GetKey(model.ProductionPlanId), model);
        }

        public async Task SetCached3M(ActiveProductionPlan3MModel model)
        {
            _responseCacheService.SetActivePlan(model);
        }

        public async Task SetCachedMachine3M(ActiveMachine3MModel model)
        {
            _responseCacheService.SetActiveMachine(model);
        }

        public async Task RemoveCached(string planId)
        {
            _responseCacheService.RemoveActivePlan(planId);
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
        /// 



        public async Task<ActiveProductionPlan3MModel> ChangeProductionStatus(string planId, PRODUCTION_PLAN_STATUS statusId)
        {
            var now = DateTime.Now;
            var activeModel = await GetCached3M(planId);
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == planId);
            var activePlan = await _activeproductionPlanRepository.FirstOrDefaultAsync(x => x.ProductionPlanPlanId == planId);
            if (activeModel?.Status != statusId)
            {
                if (statusId == PRODUCTION_PLAN_STATUS.Production)
                {
                    //close prepare process(loss)
                    var LossModel = new RecordManufacturingLossModel()
                    {
                        ProductionPlanId = planId,
                        MachineId = dbModel.MachineId
                    };
                    await _recordManufacturingLossService.End3M(LossModel);

                    //set plan status to production
                    dbModel.StatusId = activePlan.StatusId = (int)PRODUCTION_PLAN_STATUS.Production;
                    dbModel.UpdatedAt = now;
                    dbModel.UpdatedBy = CurrentUser.UserId;
                    //activePlan.StatusId = (int)PRODUCTION_PLAN_STATUS.Production;
                    _productionPlanRepository.Edit(dbModel);
                    _activeproductionPlanRepository.Edit(activePlan);
                    //setCahce
                    activeModel.Status = PRODUCTION_PLAN_STATUS.Production;
                    await SetCached3M(activeModel);
                    //return activeModel;

                    if (dbModel.MachineId != 1)//reset not guilotine
                    {
                        await _machineService.SetMachinesResetCounter3M(dbModel.MachineId, true);
                    }
                }
                await _unitOfWork.CommitAsync();
                return activeModel;
            }
            else
            {
                return null;
            }
        }

        public async Task<ActiveProductionPlan3MModel> Start(string planId, int machineId, int? target)
        {
            ActiveProductionPlan3MModel output = null;
            var cmtxt = $"Plan:{planId}";
            var step = "";
            var now = DateTime.Now;

            //validation machine is avaiable
            var mccached = _responseCacheService.GetActiveMachine(machineId);
            if (!String.IsNullOrEmpty(mccached.ProductionPlanId))
            {
                throw (new Exception($"This machine is using in Plan:{mccached.ProductionPlanId}"));
            }

            //validation
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@target", target },
            };
            var message = _directSqlRepository.ExecuteFunction<string>("dbo.fn_validation_plan_start", paramsList);
            if (message != "")
            {
                throw (new Exception(message));
            }
            else
            {
                try
                {
                    paramsList.Add("@user", CurrentUser.UserId);
                    paramsList.Add("@machineid", machineId);
                    var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_start", paramsList);
                    if (affect > 0)
                    {
                        var activeProductionPlan = (await GetCached3M(planId)) ?? new ActiveProductionPlan3MModel(planId);

                        step = "สร้าง ActiveProcess Cached";
                        activeProductionPlan.ProductionPlanId = planId;
                        activeProductionPlan.machineId = machineId;
                        activeProductionPlan.Status = PRODUCTION_PLAN_STATUS.Preparatory;

                        step = "เจน BoardcastData";
                        activeProductionPlan.ProductionData = await _dashboardService.GenerateBoardcast(DataTypeGroup.All, planId, machineId);
                        await SetCached3M(activeProductionPlan);
                        output = activeProductionPlan;

                        step = "Set machine cached";
                        mccached.ResetNewPlan(planId);
                        await _responseCacheService.SetActiveMachine(mccached);

                        if (machineId == 1)//reset guilotine
                        {
                            await _machineService.SetMachinesResetCounter3M(machineId, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var textErr = $"{cmtxt} | RecordsStep:{step} | RecordError:{ex}";
                    HelperUtility.Logging("StartPlanLog-Err.txt", textErr);
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
        public async Task<ActiveProductionPlan3MModel> Finish(string planId, int machineId)
        {
            ActiveProductionPlan3MModel output = null;
            var cmtxt = $"Plan:{planId}";
            string step = "";
            var now = DateTime.Now;
            var lastFinished = await _responseCacheService.GetAsync("LastPlanFinished");
            if (lastFinished != null && (now - JsonConvert.DeserializeObject<DateTime>(lastFinished)).TotalSeconds < _config.GetValue<int>("ProcessDelay(Sec)"))
            {
                throw (new Exception($"Please space the time foreach process {_config.GetValue<int>("ProcessDelay(Sec)")} sec.; กรุณาเว้นระยะระหว่างกระบวนการ {_config.GetValue<int>("ProcessDelay(Sec)")} วินาที"));
            }
            await _responseCacheService.SetAsync("LastPlanFinished", now);

            try
            {
                var paramsList = new Dictionary<string, object>() {
                    {"@planid", planId }
                };

                //get message for descripe validation result
                var message = _directSqlRepository.ExecuteFunction<string>("dbo.fn_validation_plan_finish", paramsList);
                if (message != "")
                {
                    step = "validation พัง";
                    throw (new Exception(message));
                }
                else
                {
                    paramsList.Add("@user", CurrentUser.UserId);
                    var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_finish", paramsList);

                    //Transaction success
                    if (affect > 0)
                    {
                        step = "Transaction success"; //HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                        var activeProductionPlan = await GetCached3M(planId);
                        if (activeProductionPlan != null)
                        {
                            step = "RemoveCached(activeProductionPlan.ProductionPlanId)"; //HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                            await RemoveCached(activeProductionPlan.ProductionPlanId);
                            activeProductionPlan.Status = Constans.PRODUCTION_PLAN_STATUS.Finished;

                            step = "delete production plan from machine";
                            var mccached = _responseCacheService.GetActiveMachine(machineId);
                            mccached.ProductionPlanId = "";
                            await _responseCacheService.SetActiveMachine(mccached);
                        }
                        step = "Reset Machine Counter";
                        await _machineService.SetMachinesResetCounter3M(machineId, true);

                        output = activeProductionPlan;
                    }
                }
            }
            catch (Exception ex)
            {
                var textErr = $"Plan:{planId} | RecordsStep:{step} | RecordError:{ex}";
                //HelperUtility.Logging("FinishPlanLog-Err.txt", textErr);
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

        public async Task<ActiveMachine3MModel> UpdateByMachine3M(int machineId, int statusId, bool isAuto)
        {
            var cachedMachine = await _machineService.GetCached3M(machineId);
            var masterData = await _masterDataService.GetData();
            var machine = masterData.Machines[machineId];

            // If Production Plan doesn't start but machine just start to send status
            if (cachedMachine == null)
            {
                cachedMachine = new ActiveMachine3MModel
                {
                    Id = machine.Id,
                    UserId = CurrentUser.UserId,
                    StartedAt = DateTime.Now
                };
            }
            cachedMachine.StatusId = statusId;
            UpdateMachineStatus(machineId, statusId);
            
            //handle machine status           
            if (cachedMachine.StatusId != statusId)
            {
                cachedMachine.StatusId = statusId;
                var paramsList = new Dictionary<string, object>() {
                    {"@planid", cachedMachine.ProductionPlanId },
                    {"@mcid", machineId },
                    {"@statusid", statusId }
                };
                _directSqlRepository.ExecuteSPNonQuery("sp_Process_Machine_Status", paramsList);
            }

            //skip when plan not production status
            if (!string.IsNullOrEmpty(cachedMachine.ProductionPlanId))
            {
                var activeProductionPlan = _responseCacheService.GetActivePlan(cachedMachine.ProductionPlanId);
                if (activeProductionPlan.Status == PRODUCTION_PLAN_STATUS.Production)
                    await HandleMachineByStatus3M(machineId, statusId, cachedMachine, isAuto);
            }

            await _unitOfWork.CommitAsync();
            await _machineService.SetCached3M(cachedMachine);

            return cachedMachine;
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

        private async Task<ActiveMachine3MModel> HandleMachineByStatus3M(int machineId, int statusId, ActiveMachine3MModel activeMachine, bool isAuto)
        {
            switch (statusId)
            {
                case Constans.MACHINE_STATUS.Stop: activeMachine = await HandleMachineStop3M(machineId, statusId, activeMachine, isAuto); break;
                case Constans.MACHINE_STATUS.Running: activeMachine = await HandleMachineRunning3M(machineId, statusId, activeMachine, isAuto); break;
                default: break;
            }
            return activeMachine;
        }

        private async Task<ActiveProductionPlanModel> HandleMachineRunning(int machineId, int statusId, ActiveProductionPlanModel activeProductionPlan, int routeId, bool isAuto)
        {
            var losses = await _recordManufacturingLossRepository
                .WhereAsync(x => x.MachineId == machineId && x.EndAt == null /*&& x.RouteId == routeId */
                                && x.IsAuto == true
                                && !(
                                        (x.LossLevel3Id == _config.GetValue<int>("DefaultChangeOverlv3Id") || x.LossLevel3Id == _config.GetValue<int>("DefaultProcessDrivenlv3Id"))
                                        && x.UpdatedAt == null
                                    )
                ); //update only isAuto = true && not rampUp

            var now = DateTime.Now;

            foreach (var dbModel in losses)
            {
                var alert = activeProductionPlan.ActiveProcesses[routeId].Alerts.FirstOrDefault(x => x.Id == Guid.Parse(dbModel.Guid));
                if (alert != null)
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

        private async Task<ActiveMachine3MModel> HandleMachineRunning3M(int machineId, int statusId, ActiveMachine3MModel activeMachine, bool isAuto)
        {
            var losses = await _recordManufacturingLossRepository
                .WhereAsync(x => x.MachineId == machineId && x.EndAt == null
                                && x.IsAuto == true
                                && !(
                                        (x.LossLevel3Id == _config.GetValue<int>("DefaultChangeOverlv3Id") || x.LossLevel3Id == _config.GetValue<int>("DefaultProcessDrivenlv3Id"))
                                        && x.UpdatedAt == null
                                    )
                ); //update only isAuto = true && not rampUp

            var now = DateTime.Now;

            foreach (var dbModel in losses)
            {
                var alert = activeMachine.Alerts.FirstOrDefault(x => x.Id == Guid.Parse(dbModel.Guid));
                if (alert != null)
                    alert.StatusId = (int)Constans.AlertStatus.Edited;

                dbModel.EndAt = now;
                dbModel.EndBy = CurrentUser.UserId;
                dbModel.Timespan = Convert.ToInt64((now - dbModel.StartedAt).TotalSeconds);
                dbModel.IsBreakdown = dbModel.Timespan >= 600;//10 minute

                _recordManufacturingLossRepository.Edit(dbModel);
                activeMachine.LossRecording = LossRecordingType.None;
            }
            return activeMachine;
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
                            //RouteId = routeId
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

        private async Task<ActiveMachine3MModel> HandleMachineStop3M(int machineId, int statusId, ActiveMachine3MModel activeMachine, bool isAuto)
        {
            var now = DateTime.Now;

            if (activeMachine.LossRecording == LossRecordingType.None) // has unclosed record inside
            {
                AlertModel alert = new AlertModel
                {
                    StatusId = (int)Constans.AlertStatus.New,
                    ItemStatusId = statusId,
                    CreatedAt = now,
                    Id = Guid.NewGuid(),
                    LossLevel3Id = _config.GetValue<int>("DefaultLosslv3Id"),
                    ItemId = machineId,
                    ItemType = (int)Constans.AlertType.MACHINE,
                };

                var paramsList = new Dictionary<string, object>() {
                            {"@startedAt", now },
                            {"@productionPlanId", activeMachine.ProductionPlanId },
                            {"@machineId", machineId },
                            {"@lossLevel3Id", _config.GetValue<int>("DefaultLosslv3Id") },
                            {"@isAuto", isAuto },
                            {"@createdBy", CurrentUser.UserId },
                            {"@guid", alert.Id.ToString() }
                        };
                _directSqlRepository.ExecuteSPNonQuery("sp_Process_ManufacturingLoss", paramsList);
                activeMachine.LossRecording = isAuto ? LossRecordingType.Auto : LossRecordingType.Manual;
                activeMachine.Alerts.Add(alert);
            }
            return activeMachine;
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

        public async Task<List<ActiveMachine3MModel>> UpdateMachineOutput(List<MachineProduceCounterModel> listData, int hour)
        {
            var machineList = new List<ActiveMachine3MModel>();
            foreach (var item in listData)
            {
                var cachedMachine = await _machineService.GetCached3M(item.MachineId);
                
                if (!string.IsNullOrEmpty(cachedMachine?.ProductionPlanId))
                {
                    var cachedProductionPlan = _responseCacheService.GetActivePlan(cachedMachine.ProductionPlanId);
                    if (cachedProductionPlan.Status == PRODUCTION_PLAN_STATUS.Production)
                    {
                        //via store
                        var paramsList = new Dictionary<string, object>() {
                            {"@planid", cachedMachine.ProductionPlanId },
                            {"@mcid", item.MachineId },
                            {"@hr", hour},
                            {"@tOut", item.CounterOut}
                        };

                        if (cachedMachine.CounterOut < item.CounterOut)
                        {                            
                            if (cachedMachine.CounterOut == 0 || cachedMachine.Hour != hour)
                            {
                                //new record
                                paramsList.Add("@isnewrecord", true);
                                paramsList.Add("@cOut", item.CounterOut - cachedMachine.CounterOut);
                                cachedMachine.CounterLastHr = cachedMachine.CounterOut;
                            }
                            else
                            {
                                //update
                                paramsList.Add("@cOut", item.CounterOut - cachedMachine.CounterLastHr);
                            }
                            _directSqlRepository.ExecuteSPNonQuery("sp_Process_Production_Counter", paramsList);
                        }

                        //set cache
                        cachedMachine.CounterOut = item.CounterOut;
                        cachedMachine.Hour = hour;
                        cachedMachine.Speed = item.Speed;
                        machineList.Add(cachedMachine);
                        await _machineService.SetCached3M(cachedMachine);
                    }
                }
            }

            return machineList;
        }

        public async Task<ActiveProductionPlanModel> AdditionalMachineOutput(string planId, int? machineId, int? routeId, int amount, int? hour, string remark)
        {
            var now = DateTime.Now;
            if (machineId != null)
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

        private void UpdateMachineStatus(int machineId, int statusId)
        {
            var mc = _machineRepository.Where(x => x.Id == machineId).FirstOrDefault();
            if (mc != null)
            {
                mc.UpdatedAt = DateTime.Now;
                mc.UpdatedBy = CurrentUser.UserId;
                mc.StatusId = statusId;
                _machineRepository.Edit(mc);
            }
        }
    }
}
