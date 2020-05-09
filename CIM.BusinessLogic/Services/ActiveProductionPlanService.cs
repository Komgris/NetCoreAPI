using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ActiveProductionPlanService : IActiveProductionPlanService
    {
        private IResponseCacheService _responseCacheService;
        private IMasterDataService _masterDataService;
        private IUnitOfWorkCIM _unitOfWork;

        public ActiveProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
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
        public async Task<ActiveProductionPlanModel> Start(ProductionPlanModel model) {
            //if (!model.RouteId.HasValue) {
            //    throw new Exception(ErrorMessages.PRODUCTION_PLAN.CANNOT_START_ROUTE_EMPTY);
            //}

            //var now = DateTime.Now;
            //var masterData = await _masterDataService.GetData();
            //if (masterData.Routes[model.RouteId.Value] == null) {
            //    throw new Exception(ErrorMessages.PRODUCTION_PLAN.CANNOT_ROUTE_INVALID);
            //}

            var output = (await this.GetCached(model.PlanId)) ?? new ActiveProductionPlanModel {
                ProductionPlanId = model.PlanId,
            };

            //var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == model.PlanId);
            //if (dbModel.StatusId == (int)Constans.PRODUCTION_PLAN_STATUS.Production) {
            //    if (output.ActiveProcesses[model.RouteId.Value] != null)
            //        throw new Exception(ErrorMessages.PRODUCTION_PLAN.PLAN_STARTED);
            //}

            //dbModel.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production;
            //dbModel.PlanStart = now;
            //dbModel.ActualStart = now;
            //dbModel.UpdatedAt = now;
            //dbModel.UpdatedBy = CurrentUser.UserId;
            //_productionPlanRepository.Edit(dbModel);

            //ValidateMachineLoss(masterData.Routes[model.RouteId.Value].MachineList);

            //_recordActiveProductionPlanRepository.Add(new RecordActiveProductionPlan {
            //    CreatedBy = CurrentUser.UserId,
            //    ProductionPlanPlanId = model.PlanId,
            //    RouteId = (int)model.RouteId,
            //    Start = now,
            //    StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production,
            //    Target = CalculateTarget(model),
            //});

            //output.ActiveProcesses[model.RouteId.Value] = new ActiveProcessModel {
            //    ProductionPlanId = model.PlanId,
            //    ProductId = model.ProductId,
            //    Route = new ActiveRouteModel {
            //        Id = model.RouteId.Value,
            //        MachineList = masterData.Routes[model.RouteId.Value].MachineList,
            //    }
            //};

            //await _machineService.BulkCacheMachines(model.PlanId, model.RouteId.Value, output.ActiveProcesses[model.RouteId.Value].Route.MachineList);
            //await _activeProductionPlanService.SetCached(output);
            //await _unitOfWork.CommitAsync();
            return output;
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
