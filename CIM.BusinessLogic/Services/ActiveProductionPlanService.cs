using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ActiveProductionPlanService : BaseService,IActiveProductionPlanService {
        private IResponseCacheService _responseCacheService;
        private IMasterDataService _masterDataService;
        private IUnitOfWorkCIM _unitOfWork;
        private IDirectSqlRepository _directSqlRepository;
        private IMachineService _machineService;
        private IRecordActiveProductionPlanRepository _recordActiveProductionPlanRepository;

        public ActiveProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IUnitOfWorkCIM unitOfWork,
            IDirectSqlRepository directSqlRepository,
            IRecordActiveProductionPlanRepository recordActiveProductionPlanRepository,
            IMachineService machineService
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _unitOfWork = unitOfWork;
            _directSqlRepository = directSqlRepository;
            _machineService = machineService;
            _recordActiveProductionPlanRepository = recordActiveProductionPlanRepository;
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

        private void ValidateMachineLoss(Dictionary<int, MachineModel> machineList) {
            //todo
            // validate mchine loss finish time stamp

            //check if machine of current route has finish = null
            //Yes-> stamp finish = now

            throw new NotImplementedException();
        }

        private int CalculateTarget(ProductionPlanModel model) {
            // todo calculate target, need final business logic
            return (int)model.Target;
        }

        public async Task<bool> Start(string planId,int routeId, int? target) {

            var now = DateTime.Now;
            var masterData = await _masterDataService.GetData();

            //already start?
            var output = (await this.GetCached(planId)) ?? new ActiveProductionPlanModel {
                ProductionPlanId = planId,
            };

            //validation
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@target", target }               
            };
            var isvalidatePass = _directSqlRepository.ExecuteFunction<bool>("dbo.fn_validation_plan_start", paramsList);

            if (isvalidatePass) {
                paramsList.Add("@user", CurrentUser.UserId);
                var affect = _directSqlRepository.ExecuteSPNonQuery("sp_process_production_start", paramsList);
                if (affect > 0) {

                    //await _machineService.BulkCacheMachines(planId, routeId, output.ActiveProcesses[routeId].Route.MachineList);
                    //await SetCached(output);
                    //await _unitOfWork.CommitAsync();

                    //ValidateMachineLoss(masterData.Routes[routeId].MachineList);

                    return true;
                }
            }

            return false;
        }

        public async Task Resume(string id, int routeId) {
            var masterData = await _masterDataService.GetData();
            ValidateMachineLoss(masterData.Routes[routeId].MachineList);
            //todo

            await _unitOfWork.CommitAsync();

        }

        public async Task Pause(string id, int routeId) {

            //var now = DateTime.Now;
            //var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == id);
            //// todo
            //// If productionstatus = Production && loss record with same planId and routeId or machineId exist, which has finsih != null  

            //_recordManufacturingLossRepository.Add(new RecordManufacturingLoss {
            //    Guid = Guid.NewGuid().ToString(),
            //    CreatedBy = CurrentUser.UserId,
            //    LossLevel3Id = Constans.DEFAULT_LOSS_LV3,
            //    RouteId = routeId,
            //});

            //var activeProductionPlan = await _activeProductionPlanService.GetCached(id);
            //if (activeProductionPlan != null) {
            //    var activeProcess = activeProductionPlan.ActiveProcesses[routeId];
            //    if (activeProcess == null)
            //        throw new Exception($"Route {routeId} is not active in PlanId {id}.");

            //    foreach (var machine in activeProcess.Route.MachineList) {
            //        machine.Value.RouteList.Remove(routeId);
            //        // todo
            //        // if machine is no long in any route -> insert record_manufactoring_loss
            //        await _machineService.RemoveCached(machine.Key, null);
            //    }
            //    activeProductionPlan.ActiveProcesses.Remove(activeProcess.Route.Id);
            //}
            await _unitOfWork.CommitAsync();

        }

        public async Task Finish(ProductionPlanModel model) {

            //var now = DateTime.Now;
            //var masterData = await _masterDataService.GetData();
            //var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == model.PlanId);
            //await _recordManufacturingLossService.Create(new RecordManufacturingLossModel {
            //    Guid = Guid.NewGuid().ToString(),
            //    CreatedBy = CurrentUser.UserId,
            //    LossLevel3Id = Constans.DEFAULT_LOSS_LV3,

            //});
            //dbModel.UpdatedAt = now;
            //dbModel.UpdatedBy = CurrentUser.UserId;
            //_productionPlanRepository.Edit(dbModel);

            //var activeProductionPlan = await _activeProductionPlanService.GetCached(model.PlanId);

            //if (activeProductionPlan != null) {
            //    var activeProcess = activeProductionPlan.ActiveProcesses[(int)model.RouteId];
            //    if (activeProcess == null)
            //        throw new Exception($"Route {model.RouteId} is not active in PlanId {model.PlanId}. ");


            //    foreach (var machine in activeProcess.Route.MachineList) {
            //        await _machineService.RemoveCached(machine.Key, null);
            //    }
            //    activeProductionPlan.ActiveProcesses.Remove(activeProcess.Route.Id);
            //    if (activeProductionPlan.ActiveProcesses.Count() == 0)
            //        await _activeProductionPlanService.RemoveCached(activeProductionPlan.ProductionPlanId);
            //}
            await _unitOfWork.CommitAsync();

        }

    }
}
