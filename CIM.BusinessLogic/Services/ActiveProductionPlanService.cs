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

        public async Task<ActiveMachine3MModel> GetCachedMachine3M(int machineId)
        {
            return _responseCacheService.GetActiveMachine(machineId);
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
            if (mccached.ProductionPlanId != "")
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
                    //var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == planId);
                    paramsList.Add("@user", CurrentUser.UserId);
                    paramsList.Add("@machineid", machineId);
                    var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_start", paramsList);
                    //var affect = 1;
                    if (affect > 0)
                    {
                        var masterData = await _masterDataService.GetData();
                        var activeProductionPlan = (await GetCached3M(planId)) ?? new ActiveProductionPlan3MModel(planId);

                        step = "สร้าง active Machine";
                        Dictionary<int, ActiveMachine3MModel> machine = new Dictionary<int, ActiveMachine3MModel>();
                        var active = new ActiveMachine3MModel
                        {
                            ProductionPlanId = planId,
                            Id = machineId,
                            StatusId = Constans.MACHINE_STATUS.Idle
                        };
                        machine.Add(machineId, active);
                        var activeMachines = await _machineService.BulkCacheMachines3M(planId, machineId);


                        step = "สร้าง ActiveProcess Cached";
                        activeProductionPlan.ProductionPlanId = planId;
                        activeProductionPlan.ProductId = machineId;
                        activeProductionPlan.Status = PRODUCTION_PLAN_STATUS.Preparatory;
                        //activeProductionPlan.Machine = activeMachines;


                        step = "Change idle to Running";
                        var runningMachineIds = machine.Where(x => x.Value.StatusId == Constans.MACHINE_STATUS.Idle).Select(x => x.Key).ToArray();
                        foreach (var mcId in runningMachineIds)
                        {
                            _recordMachineStatusRepository.Add(new RecordMachineStatus
                            {
                                CreatedAt = now,
                                MachineId = mcId,
                                MachineStatusId = Constans.MACHINE_STATUS.Running,
                            });
                            UpdateMachineStatus(mcId, Constans.MACHINE_STATUS.Running);
                        }

                        step = "List for Reset counter";
                        //var mcfirstStart = machine.Where(x => x.Value.RouteIds.Count == 1).Select(o => o.Key).ToList();
                        //await _machineService.SetListMachinesResetCounter(mcfirstStart, true);

                        step = "เจน BoardcastData";
                        activeProductionPlan.ProductionData = await _dashboardService.GenerateBoardcast(DataTypeGroup.All, planId, machineId);
                        await SetCached3M(activeProductionPlan);
                        output = activeProductionPlan;
                        //var lstr = output.ActiveProcesses.Select(o => o.Key).ToList();

                        step = "Add Preparatory";
                        //record time loss on process ramp-up #139
                        var rampupModel = new RecordManufacturingLossModel()
                        {
                            ProductionPlanId = planId,
                            MachineId = machineId,
                            LossLevelId = _config.GetValue<int>("DefaultProcessDrivenlv3Id"),
                            IsAuto = true
                        };
                        await _recordManufacturingLossService.Create3M(rampupModel);

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
        public async Task<ActiveProductionPlanModel> Finish(string planId, int machineId)
        {
            ActiveProductionPlanModel output = null;
            var cmtxt = $"Plan:{planId} | Machine:{machineId}";
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
                    {"@planid", planId },
                    {"@routeid", machineId }
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
                        step = "Transaction success"; HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                        var activeProductionPlan = await GetCached(planId);
                        if (activeProductionPlan != null)
                        {
                            step = "GetCached activeProductionPlan ไม่ได้"; HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                            var mcliststopCounting = new List<int>();
                            var activeProcess = activeProductionPlan.ActiveProcesses[machineId];
                            activeProductionPlan.ActiveProcesses[machineId].Status = Constans.PRODUCTION_PLAN_STATUS.Finished;
                            var isPlanActive = activeProductionPlan.ActiveProcesses.Count(x => x.Value.Status != Constans.PRODUCTION_PLAN_STATUS.Finished) > 0;

                            //update another route are use the same machines
                            step = "loop mcmultiRoute"; HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                            foreach (var mcmultiRoute in activeProcess.Route.MachineList.Where(a => a.Value.RouteIds.Count > 1))
                            {
                                foreach (var r in mcmultiRoute.Value.RouteIds)
                                {
                                    if (r != machineId)
                                        activeProductionPlan.ActiveProcesses[r].Route.MachineList[mcmultiRoute.Key].RouteIds.Remove(machineId);
                                }
                            }
                            step = "loop MachineList"; HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                            foreach (var machine in activeProcess.Route.MachineList)
                            {
                                machine.Value.RouteIds.Remove(machineId);
                                if (machine.Value.RouteIds.Count == 0) mcliststopCounting.Add(machine.Key);
                                if (!isPlanActive) machine.Value.ProductionPlanId = null;
                                await _machineService.SetCached(machine.Key, machine.Value);
                            }


                            //stop counting output
                            step = "loop SetListMachinesResetCounter"; HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                            await _machineService.SetListMachinesResetCounter(mcliststopCounting, false);
                            if (isPlanActive)
                            {
                                step = "SetCached(activeProductionPlan)"; HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                                await SetCached(activeProductionPlan);
                            }
                            else
                            {
                                step = "RemoveCached(activeProductionPlan.ProductionPlanId)"; HelperUtility.Logging("FinishPlanLog.txt", $"{cmtxt} | {step}");
                                await RemoveCached(activeProductionPlan.ProductionPlanId);
                                activeProductionPlan.Status = Constans.PRODUCTION_PLAN_STATUS.Finished;

                                step = "delete production plan from machine";
                                var mccached = _responseCacheService.GetActiveMachine(machineId);
                                mccached.ProductionPlanId = "";
                                await _responseCacheService.SetActiveMachine(mccached);
                            }
                        }
                        output = activeProductionPlan;
                    }
                }
            }
            catch (Exception ex)
            {
                var textErr = $"Plan:{planId} | Machine:{machineId} | RecordsStep:{step} | RecordError:{ex}";
                HelperUtility.Logging("FinishPlanLog-Err.txt", textErr);
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

        public async Task<ActiveProductionPlan3MModel> UpdateByMachine3M(int machineId, int statusId, bool isAuto)
        {
            var now = DateTime.Now;
            var activeRoute = new List<int>();
            var cachedMachine = await _machineService.GetCached3M(machineId);
            var masterData = await _masterDataService.GetData();
            var machine = masterData.Machines[machineId];
            ActiveProductionPlan3MModel output = null;

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

            //if machine is apart of production plan
            if (!string.IsNullOrEmpty(cachedMachine.ProductionPlanId) /*&& cachedMachine.RouteIds != null*/)
            {
                output = await GetCached3M(cachedMachine.ProductionPlanId);
                if (output != null)
                {
                    //machine CH


                    //activeprocess CH

                    //foreach (var routeId in cachedMachine.RouteIds.Distinct())
                    //{
                    //if (output.ActiveProcesses.ContainsKey(machineId))
                    //{
                    //    UpdateMachineStatus(machineId, statusId);
                    //    activeRoute.Add(machineId);

                    //    output.Machine.StatusId = statusId;
                    //    output = await HandleMachineByStatus3M(machineId, statusId, output, isAuto);
                    //}
                    //}
                    await SetCached3M(output);
                }
            }

            await SetCachedMachine3M(cachedMachine);

            //handle machine status
            //var recordMachineStatusId = statusId;
            //if (string.IsNullOrEmpty(cachedMachine.ProductionPlanId) && statusId == Constans.MACHINE_STATUS.Running)
            //{
            //    recordMachineStatusId = Constans.MACHINE_STATUS.Idle;
            //}

            //if (cachedMachine.StatusId != recordMachineStatusId)
            //{
            //    cachedMachine.StatusId = recordMachineStatusId;
            //    var paramsList = new Dictionary<string, object>() {
            //        {"@planid", cachedMachine.ProductionPlanId },
            //        {"@mcid", machineId },
            //        {"@statusid", recordMachineStatusId }
            //    };
            //    _directSqlRepository.ExecuteSPNonQuery("sp_Process_Machine_Status", paramsList);
            //}

            await _unitOfWork.CommitAsync();
            await _machineService.SetCached3M(cachedMachine);

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

        private async Task<ActiveProductionPlan3MModel> HandleMachineByStatus3M(int machineId, int statusId, ActiveProductionPlan3MModel activeProductionPlan, bool isAuto)
        {
            switch (statusId)
            {
                case Constans.MACHINE_STATUS.Stop: activeProductionPlan = await HandleMachineStop3M(machineId, statusId, activeProductionPlan, isAuto); break;
                case Constans.MACHINE_STATUS.Running: activeProductionPlan = await HandleMachineRunning3M(machineId, statusId, activeProductionPlan, isAuto); break;
                default: break;
            }
            return activeProductionPlan;
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

        private async Task<ActiveProductionPlan3MModel> HandleMachineRunning3M(int machineId, int statusId, ActiveProductionPlan3MModel activeProductionPlan, bool isAuto)
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
                //var alert = activeProductionPlan.ActiveProcesses[machineId].Alerts.FirstOrDefault(x => x.Id == Guid.Parse(dbModel.Guid));
                //if (alert != null)
                //    alert.EndAt = now;
                dbModel.EndAt = now;
                dbModel.EndBy = CurrentUser.UserId;
                dbModel.Timespan = Convert.ToInt64((now - dbModel.StartedAt).TotalSeconds);
                dbModel.IsBreakdown = dbModel.Timespan >= 600;//10 minute
                if (dbModel.Timespan < 60 && dbModel.IsAuto)
                {
                    dbModel.LossLevel3Id = _config.GetValue<int>("DefaultSpeedLosslv3Id");
                    //var sploss = activeProductionPlan.ActiveProcesses[machineId].Alerts.FirstOrDefault(x => x.Id == Guid.Parse(dbModel.Guid));
                    //handle case alert is removed from redis
                    //if (sploss != null)
                    //{
                    //    sploss.LossLevel3Id = dbModel.LossLevel3Id;
                    //    sploss.StatusId = (int)Constans.AlertStatus.Edited;
                    //}
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

        private async Task<ActiveProductionPlan3MModel> HandleMachineStop3M(int machineId, int statusId, ActiveProductionPlan3MModel activeProductionPlan, bool isAuto)
        {
            var now = DateTime.Now;

            var cachedMachine = await _machineService.GetCached3M(machineId);

            //if (!activeProductionPlan.ActiveProcesses[machineId].Machine.IsReady) // has unclosed record inside
            //{
            AlertModel alert;


            alert = new AlertModel
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
                            {"@productionPlanId", activeProductionPlan.ProductionPlanId },
                            {"@machineId", machineId },
                            {"@lossLevel3Id", _config.GetValue<int>("DefaultLosslv3Id") },
                            {"@isAuto", isAuto },
                            {"@createdBy", CurrentUser.UserId },
                            {"@guid", alert.Id.ToString() }
                        };
            _directSqlRepository.ExecuteSPNonQuery("sp_Process_ManufacturingLoss", paramsList);


            //activeProductionPlan.ActiveProcesses[machineId].Alerts.Add(alert);

            //}

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

                    if (dbOutput.Count > 0 && dbOutput[0].TotalOut > item.CounterOut)
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

                                //// Logging
                                //var textErr = $"Plan:{cachedMachine.ProductionPlanId} | Machine:{item.MachineId} | Hour:{dbOutput[0].Hour}<->{hour} | RecordsCnt:{dbOutput.Count}";
                                //HelperUtility.Logging("CounterAdd_logging.txt", textErr);

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
