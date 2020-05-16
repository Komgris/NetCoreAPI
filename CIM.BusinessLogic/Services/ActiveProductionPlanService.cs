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
        private IActiveProductionPlanService _activeProductionPlanService;
        private IProductionPlanRepository _productionPlanRepository;

        public ActiveProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IUnitOfWorkCIM unitOfWork,
            IDirectSqlRepository directSqlRepository,
            IRecordActiveProductionPlanRepository recordActiveProductionPlanRepository,
            IMachineService machineService,
            IActiveProductionPlanService activeProductionPlanService,
            IProductionPlanRepository productionPlanRepository
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _unitOfWork = unitOfWork;
            _directSqlRepository = directSqlRepository;
            _machineService = machineService;
            _recordActiveProductionPlanRepository = recordActiveProductionPlanRepository;
            _activeProductionPlanService = activeProductionPlanService;
            _productionPlanRepository = productionPlanRepository;


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
        public async Task<bool> Start(string planId,int routeId, int? target) {

            //var now = DateTime.Now;
            //var masterData = await _masterDataService.GetData();

            //already start?
            //var output = (await this.GetCached(planId)) ?? new ActiveProductionPlanModel {
            //    ProductionPlanId = planId,
            //};
            return await Task.Run(async () => {

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

                        var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == planId);
                        var masterData = await _masterDataService.GetData();
                        var activeProductionPlan = (await _activeProductionPlanService.GetCached(planId)) ?? new ActiveProductionPlanModel
                        {
                            ProductionPlanId = planId,
                        };

                        activeProductionPlan.ActiveProcesses[routeId] = new ActiveProcessModel
                        {
                            ProductionPlanId = planId,
                            ProductId = dbModel.ProductId,
                            Route = new ActiveRouteModel
                            {
                                Id = routeId,
                                MachineList = masterData.Routes[routeId].MachineList,
                            }
                        };
                        await _machineService.BulkCacheMachines(planId, routeId, activeProductionPlan.ActiveProcesses[routeId].Route.MachineList);
                        await _activeProductionPlanService.SetCached(activeProductionPlan);

                        return true;
                    }
                }
                return false;
            });

        }

        public async Task<bool> Pause(string planId, int routeId) {

            return await Task.Run(() => {

                //validation
                var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };
                var isvalidatePass = _directSqlRepository.ExecuteFunction<bool>("dbo.fn_Validation_Plan_Pause", paramsList);
                if (isvalidatePass) {
                    return true;
                }
                return false;
            });
        }

        public async Task<bool> Resume(string planId, int routeId) {

            return await Task.Run(() => {

                if (true) {
                    return true;
                }
                return false;
            });
        }

        public async Task<bool> Finish(string planId, int routeId) {

            return await Task.Run(() => {
                //validation
                var paramsList = new Dictionary<string, object>() {
                    {"@planid", planId },
                    {"@routeid", routeId }
                };

                var isvalidatePass = _directSqlRepository.ExecuteFunction<bool>("dbo.fn_Validation_Plan_Finish", paramsList);
                if (isvalidatePass) {
                    return true;
                }
                return false;
            });
        }
    }
}
