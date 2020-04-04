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

        public async Task<PagingModel<ProductionPlanListModel>> List(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive)
        {
            var output = await _productionPlanRepository.ListAsPaging(page, howmany, keyword, productId, routeId, isActive);
            return output;
        }


        public async Task<List<ProductionPlanModel>> Insert(List<ProductionPlanModel> import)
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
            _productionPlanRepository.Delete(existingItem);
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
                    plan.Status = "Inprocess";
                }
                else
                {
                    plan.Status = "New";
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

        public async Task Start(ProductionPlanModel model)
        {
            var now = DateTime.Now;
            var masterData = await _masterDataService.GetData();
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == model.PlanId);

            if (dbModel.Status == Constans.PRODUCTION_PLAN_STATUS.STARTED)
            {
                throw new Exception(ErrorMessages.PRODUCTION_PLAN.PLAN_STARTED);
            }

            if (!model.RouteId.HasValue)
            {
                throw new Exception(ErrorMessages.PRODUCTION_PLAN.CANNOT_START_ROUTE_EMPTY);
            }

            if (masterData.Routes[model.RouteId.Value] == null)
            {
                throw new Exception(ErrorMessages.PRODUCTION_PLAN.CANNOT_ROUTE_INVALID);
            }

            dbModel.RouteId = model.RouteId;
            dbModel.Status = Constans.PRODUCTION_PLAN_STATUS.STARTED;
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
                MachineList = route.MachineList,
            };
            foreach (var machine in activeProcess.Route.MachineList)
            {
                foreach (var component in machine.Value.ComponentList)
                {
                    var cachedComponent = await _responseCacheService.GetAsTypeAsync<ActiveComponentModel>($"{Constans.RedisKey.COMPONENT}:{component.Key}");
                    if (cachedComponent == null)
                    {
                        cachedComponent = new ActiveComponentModel
                        {
                            MachineComponentId = component.Key,
                            MachineId = machine.Key,
                            StatusId = (int)Constans.ComponentStatus.Ready
                        };
                    }
                    cachedComponent.ProductionPlanId = activeProcess.ProductionPlanId;


                    await _responseCacheService.SetAsync($"{Constans.RedisKey.COMPONENT}:{component.Key}", cachedComponent);
                }
            }
            await _responseCacheService.SetAsync($"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{activeProcess.ProductionPlanId}", activeProcess);
            await _unitOfWork.CommitAsync();

        }

        public async Task Stop(string id)
        {

            var now = DateTime.Now;
            var masterData = await _masterDataService.GetData();
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == id);

            dbModel.Status = Constans.PRODUCTION_PLAN_STATUS.STOP;
            dbModel.UpdatedAt = now;
            dbModel.UpdatedBy = CurrentUser.UserId;
            _productionPlanRepository.Edit(dbModel);

            //to handle cache data and boardcast
            await _unitOfWork.CommitAsync();

        }

        public async Task<ProductionPlanModel> Load(string id)
        {
            var masterData = await _masterDataService.GetData();
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == id);
            var productDb = await _productRepository.FirstOrDefaultAsync(x => x.Id == dbModel.ProductId);
            var model = MapperHelper.AsModel(dbModel, new ProductionPlanModel(), new [] { "Product"});
            model.Product = MapperHelper.AsModel(productDb, new ProductModel());
            return model;
        }

        public async Task<ProductionPlanModel> Get(string planId)
        {
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == planId);
            return MapperHelper.AsModel(dbModel, new ProductionPlanModel());
        }

        public async Task<ActiveProcessModel> UpdateByComponent(int id, int statusId)
        {
            var cachedComponent = await _responseCacheService.GetAsTypeAsync<ActiveComponentModel>($"{Constans.RedisKey.COMPONENT}:{id}");
            ActiveProcessModel productionPlan = null;
            // If Production Plan doesn't start but component just start to send status
            if (cachedComponent == null)
            {
                var masterData = await _masterDataService.GetData();
                var component = masterData.Components[id];
                cachedComponent = new ActiveComponentModel
                {
                    MachineComponentId = component.Id,
                    MachineId = component.MachineId,
                };
                await _responseCacheService.SetAsync($"{Constans.RedisKey.COMPONENT}:{component.Id}", cachedComponent);
            }
            productionPlan = await _responseCacheService.GetAsTypeAsync<ActiveProcessModel>($"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{cachedComponent.ProductionPlanId}");
            bool hasPropductionPlanStarted = productionPlan != null;

            if (hasPropductionPlanStarted)
            {
                
                productionPlan.Alerts.Add(new AlertModel
                {

                    CreatedAt = DateTime.Now,
                    ComponentStatusId = statusId,
                    ItemId = id,
                    ItemType = (int)Constans.AlertType.Component,
                    StatusId = (int)Constans.AlertStatus.New,

                });

                productionPlan.Route.MachineList[cachedComponent.MachineId].ComponentList[cachedComponent.MachineComponentId].Status = statusId;
                await _responseCacheService.SetAsync($"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{productionPlan.ProductId}", productionPlan);
            }

            return productionPlan;
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
