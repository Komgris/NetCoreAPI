using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using CIM.BusinessLogic.Utility;
using CIM.Domain.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace CIM.BusinessLogic.Services
{
    public class ProductionPlanService : BaseService, IProductionPlanService
    {
        private IResponseCacheService _responseCacheService;
        private IMasterDataService _masterDataService;
        private IProductionPlanRepository _productionPlanRepository;
        private IProductRepository _productRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public ProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IUnitOfWorkCIM unitOfWork,
            IProductionPlanRepository productionPlanRepository,
            IProductRepository productRepository
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _productionPlanRepository = productionPlanRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public List<ProductionPlanModel> Get()
        {
            var db = _productionPlanRepository.All().ToList();
            List<ProductionPlanModel> productDb = new List<ProductionPlanModel>();
            foreach (var plan in db)
            {
                var db_model = MapperHelper.AsModel(plan, new ProductionPlanModel());
                productDb.Add(db_model);
            }
            return productDb;
        }

        public async Task<PagingModel<ProductionPlanModel>> Paging(int page, int howmany)
        {
            var plan = await _productionPlanRepository.WhereAsync(x => x.IsActive == true);
            int total = plan.Count();

            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;

            var dbModel = plan.OrderBy(x => x.PlanId).Skip(skipRec).Take(takeRec).ToList();

            var output = new List<ProductionPlanModel>();
            foreach (var item in dbModel)
            {
                output.Add(MapperHelper.AsModel(item, new ProductionPlanModel()));
            }
            return new PagingModel<ProductionPlanModel>
            {
                HowMany = total,
                Data = output
            };
        }

        public async Task<PagingModel<ProductionPlanListModel>> List(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive, string statusIds)
        {
            var output = await _productionPlanRepository.ListAsPaging(page, howmany, keyword, productId, routeId, isActive, statusIds);
            return output;
        }


        public async Task<List<ProductionPlanModel>> Create(List<ProductionPlanModel> import)
        {
            List<ProductionPlanModel> fromDb = _productionPlanRepository.Get();
            List<ProductionPlanModel> db_list = new List<ProductionPlanModel>();
            List<ProductionPlanModel> existsPlan = new List<ProductionPlanModel>();
            DateTime timeNow = DateTime.Now;
            foreach (var plan in import)
            {
                if (fromDb.Any(x => x.PlanId == plan.PlanId))
                {
                    var db_model = MapperHelper.AsModel(plan, new ProductionPlan());
                    db_model.UpdatedBy = CurrentUser.UserId;
                    db_model.UpdatedAt = timeNow;
                    _productionPlanRepository.Edit(db_model);
                }
                else
                {
                    var db_model = MapperHelper.AsModel(plan, new ProductionPlan());
                    db_model.CreatedBy = CurrentUser.UserId;
                    db_model.CreatedAt = timeNow;
                    db_model.IsActive = true;
                    _productionPlanRepository.Add(db_model);
                    db_list.Add(MapperHelper.AsModel(db_model, new ProductionPlanModel()));
                }
            }
            await _unitOfWork.CommitAsync();
            return db_list;
        }

        public async Task Delete(string id)
        {
            var existingItem = _productionPlanRepository.Where(x => x.PlanId == id).ToList().FirstOrDefault();
            existingItem.IsActive = false;
            await _unitOfWork.CommitAsync();
        }

        public void Update(List<ProductionPlanModel> list)
        {
            _productionPlanRepository.UpdateProduction(list);
        }

        public List<ProductionPlanModel> Compare(List<ProductionPlanModel> import, List<ProductionPlanModel> dbPlan)
        {
            foreach (var plan in import)
            {
                if (dbPlan.Any(x => x.PlanId == plan.PlanId))
                {
                    //plan.StatusId = "Inprocess";
                }
                else
                {
                    plan.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.New;
                }
            }
            return import;
        }

        public List<ProductionPlanModel> ReadImport(string path)
        {
            FileInfo excel = new FileInfo(path);
            using (var package = new ExcelPackage(excel))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.First();
                List<ProductionPlanModel> intList = ConvertImportToList(worksheet);
                return intList;
            }
        }

        public List<ProductionPlanModel> ConvertImportToList(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            List<ProductionPlanModel> listImport = new List<ProductionPlanModel>();
            for (int i = 2; i <= totalRows; i++)
            {
                ProductionPlanModel data = new ProductionPlanModel();
                data.PlanId = (oSheet.Cells[i, 1].Value ?? string.Empty).ToString();
                data.ProductId = Convert.ToInt32(oSheet.Cells[i, 2].Value ?? string.Empty);
                data.Target = Convert.ToInt32(oSheet.Cells[i, 3].Value ?? string.Empty);
                data.Unit = Convert.ToInt32(oSheet.Cells[i, 4].Value ?? string.Empty);
                listImport.Add(data);
            }
            return listImport;
        }

        private string GetProductionPlanRouteKey(string id, int routeId)
        {
            return $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{id}:{routeId}";
        }

        private string GetProductionPlanKey(string id)
        {
            return $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{id}";
        }

        public async Task<ActiveProcessModel> Start(ProductionPlanModel model)
        {
            var now = DateTime.Now;
            var masterData = await _masterDataService.GetData();
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == model.PlanId);

            ActiveProcessModel productionPlan;
            if (!model.RouteId.HasValue)
            {
                throw new Exception(ErrorMessages.PRODUCTION_PLAN.CANNOT_START_ROUTE_EMPTY);
            }

            var productionPlanRouteKey = GetProductionPlanRouteKey(model.PlanId, model.RouteId.Value);

            if (masterData.Routes[model.RouteId.Value] == null)
            {
                throw new Exception(ErrorMessages.PRODUCTION_PLAN.CANNOT_ROUTE_INVALID);
            }

            if (dbModel.StatusId == (int)Constans.PRODUCTION_PLAN_STATUS.Production)
            {
                productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProcessModel>(productionPlanRouteKey);
                if (productionPlan.Route.Id == model.RouteId)
                    throw new Exception(ErrorMessages.PRODUCTION_PLAN.PLAN_STARTED);
            }

            dbModel.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production;
            dbModel.PlanStart = now;
            dbModel.ActualStart = now;
            dbModel.UpdatedAt = now;
            dbModel.UpdatedBy = CurrentUser.UserId;
            _productionPlanRepository.Edit(dbModel);

            var activeProcess = new ActiveProcessModel
            {
                ProductionPlanId = model.PlanId,
                ProductId = model.ProductId,
            };

            var route = masterData.Routes[model.RouteId.Value];
            activeProcess.Route = new ActiveRouteModel
            {
                Id = route.Id,
                MachineList = route.MachineList,
            };

            foreach (var machine in activeProcess.Route.MachineList)
            {

                var cachedMachine = await _responseCacheService.GetAsTypeAsync<ActiveMachineModel>($"{Constans.RedisKey.MACHINE}:{machine.Key}");
                if (cachedMachine == null)
                {
                    cachedMachine = new ActiveMachineModel
                    {
                        Id = machine.Key,
                        ProductionPlanId = model.PlanId,
                        RouteIds = new List<int> { model.RouteId.Value }
                    };
                    await _responseCacheService.SetAsync($"{Constans.RedisKey.MACHINE}:{machine.Key}", cachedMachine);
                }
                else
                {
                    cachedMachine.RouteIds.Add(model.RouteId.Value);
                }

            }
            var productionPlanKey = GetProductionPlanKey(model.PlanId);
            var cachedProductionPlanRoute = (await _responseCacheService.GetAsTypeAsync<List<int>>(productionPlanKey)) ?? new List<int>();
            cachedProductionPlanRoute.Add(model.RouteId.Value);
            await _responseCacheService.SetAsync(productionPlanKey, cachedProductionPlanRoute.Distinct().ToArray());
            await _responseCacheService.SetAsync(productionPlanRouteKey, activeProcess);
            await _unitOfWork.CommitAsync();
            return activeProcess;
        }

        public async Task Stop(string id, int[] routeIds)
        {

            var now = DateTime.Now;
            var masterData = await _masterDataService.GetData();
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == id);

            dbModel.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Finished;
            dbModel.UpdatedAt = now;
            dbModel.UpdatedBy = CurrentUser.UserId;
            _productionPlanRepository.Edit(dbModel);

            var productionPlanKey = GetProductionPlanKey(id);
            if (routeIds.Count() == 0)
            {
                routeIds = (await _responseCacheService.GetAsTypeAsync<List<int>>(productionPlanKey)).ToArray();
            }

            foreach (var routeId in routeIds)
            {
                var productionPlanRouteKey = GetProductionPlanRouteKey(id, routeId);
                var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProcessModel>(productionPlanRouteKey);
                if (productionPlan != null)
                {
                    foreach (var machine in productionPlan.Route.MachineList)
                    {
                        await _responseCacheService.SetAsync($"{Constans.RedisKey.MACHINE}:{machine.Key}", null);
                    }
                    await _responseCacheService.SetAsync(productionPlanRouteKey, null);
                }
            }
            await _responseCacheService.SetAsync(productionPlanKey, null);
            //to handle cache data and boardcast
            await _unitOfWork.CommitAsync();

        }

        public async Task<ProductionPlanModel> Load(string id)
        {
            var masterData = await _masterDataService.GetData();
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == id);
            var productDb = await _productRepository.FirstOrDefaultAsync(x => x.Id == dbModel.ProductId);
            var model = MapperHelper.AsModel(dbModel, new ProductionPlanModel(), new[] { "Product" });
            model.Product = MapperHelper.AsModel(productDb, new ProductModel());
            return model;
        }

        public async Task<ProductionPlanModel> Get(string planId)
        {
            var output = await _productionPlanRepository.Where( x=>x.PlanId == planId).Select(
                        x => new ProductionPlanModel
                        {
                            PlanId = x.PlanId,
                            ProductId = x.ProductId,
                            ProductCode = x.Product.Code,
                            RouteId = x.RouteId,
                            Route = x.Route.Name,
                            Target = x.Target,
                            Unit = x.UnitId,
                            UnitName = x.Unit.Name,
                            PlanStart = x.PlanStart,
                            PlanFinish = x.PlanFinish,
                            ActualStart = x.ActualStart,
                            ActualFinish = x.ActualFinish,
                            StatusId = x.StatusId,
                            Status = x.Status.Name,
                            IsActive = x.IsActive,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                            UpdatedBy = x.UpdatedBy,
                        }).FirstOrDefaultAsync();
            return output;
        }


        public async Task<ActiveProcessModel> TakeAction(int id)
        {
            var productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProcessModel>($"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{id}");

            foreach (var item in productionPlan.Alerts)
            {
                item.StatusId = (int)Constans.AlertStatus.Processing;
            }
            await _responseCacheService.SetAsync($"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan.ProductId}", productionPlan);
            return productionPlan;
        }

    }
}
