﻿using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using CIM.BusinessLogic.Utility;
using CIM.Domain.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.Internal;

namespace CIM.BusinessLogic.Services
{
    public class ProductionPlanService : BaseService, IProductionPlanService
    {
        private IResponseCacheService _responseCacheService;
        private IMasterDataService _masterDataService;
        private IProductionPlanRepository _productionPlanRepository;
        private IProductRepository _productRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private IMachineService _machineService;
        private IActiveProductionPlanService _activeProductionPlanService;
        private IRecordManufacturingLossService _recordManufacturingLossService;
        private IRecordActiveProductionPlanRepository _recordActiveProductionPlanRepository;
        private IRecordManufacturingLossRepository _recordManufacturingLossRepository;
        private IReportService _reportService;

        public ProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IUnitOfWorkCIM unitOfWork,
            IProductionPlanRepository productionPlanRepository,
            IProductRepository productRepository,
            IMachineService machineService,
            IActiveProductionPlanService activeProductionPlanService,
            IRecordManufacturingLossService recordManufacturingLossService,
            IRecordActiveProductionPlanRepository recordActiveProductionPlanRepository,
            IRecordManufacturingLossRepository recordManufacturingLossRepository,
            IReportService reportService
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _productionPlanRepository = productionPlanRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _machineService = machineService;
            _activeProductionPlanService = activeProductionPlanService;
            _recordManufacturingLossService = recordManufacturingLossService;
            _recordActiveProductionPlanRepository = recordActiveProductionPlanRepository;
            _recordManufacturingLossRepository = recordManufacturingLossRepository;
            _reportService = reportService;
        }

        private static class ExcelMapping
        {
            public const int PLAN = 3;
            public const int ROUTE = 4;
            public const int PRODUCT = 5;
            public const int TARGET = 15;
            public const int UNIT = 16;
            public const int PLANSTART = 17;
            public const int PLANFINISH = 18;
            public const int OFFSET_TOP_ROW = 5;
            public const int OFFSET_BOTTOM_ROW = 2;
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


        public async Task<List<ProductionPlanModel>> CheckDuplicate(List<ProductionPlanModel> import)
        {
            List<ProductionPlanModel> db_list = new List<ProductionPlanModel>();
            var masterData = await _masterDataService.GetData();
            var productionPlanDict = masterData.ProductionPlan;
            foreach (var plan in import)
            {
                if (productionPlanDict.ContainsKey(plan.PlanId))
                {
                     UpdatePlan(plan);
                }
                else
                {
                    var model =  CreatePlan(plan);
                    db_list.Add(model);
                }
            }
            await _unitOfWork.CommitAsync();
            return db_list;
        }

        public async Task<ProductionPlanModel> Create(ProductionPlanModel model)
        {
            var plan = CreatePlan(model);
            await _unitOfWork.CommitAsync();
            return plan;
        }

        public async Task Update(ProductionPlanModel model)
        {
            UpdatePlan(model);
            await _unitOfWork.CommitAsync();
        }

        public ProductionPlanModel CreatePlan(ProductionPlanModel model)
        {
            var db_model = MapperHelper.AsModel(model, new ProductionPlan(), new[] { "Route", "Unit" });
            db_model.UnitId = model.Unit;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.CreatedAt = DateTime.Now;
            db_model.IsActive = true;
            db_model.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.New;
            _productionPlanRepository.Add(db_model);
            return (MapperHelper.AsModel(db_model, new ProductionPlanModel()));
        }

        public void UpdatePlan(ProductionPlanModel model)
        {
            var db_model = MapperHelper.AsModel(model, new ProductionPlan(), new[] { "Route", "Product", "Status", "Unit" });
            db_model.UnitId = model.Unit;
            db_model.UpdatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.UpdatedAt = DateTime.Now;
            _productionPlanRepository.Edit(db_model);
        }

        public async Task Delete(string id)
        {
            var existingItem = _productionPlanRepository.Where(x => x.PlanId == id).ToList().FirstOrDefault();
            existingItem.IsActive = false;
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<ProductionPlanModel>> Compare(List<ProductionPlanModel> import)
        {
            var masterData = await _masterDataService.GetData();
            var productionPlanDict = masterData.ProductionPlan;
            var productDict = masterData.Dictionary.Products;
            var productCodeToIds = masterData.Dictionary.ProductsByCode;
            var activeProductionPlanOutput = _reportService.GetActiveProductionPlanOutput();
            var timeBuffer = (int)Constans.ProductionPlanBuffer.HOUR_BUFFER;
            var targetBuffer = (int)Constans.ProductionPlanBuffer.TARGET_BUFFER;

            DateTime timeNow = DateTime.Now;

            foreach (var plan in import)
            {
                plan.ProductId = ProductCodeToId(plan.ProductCode, productCodeToIds);
                if (plan.ProductId != 0)
                {
                    if (productionPlanDict.ContainsKey(plan.PlanId))
                    {
                        plan.StatusId = await planStatus(plan.PlanId);
                        plan.CompareResult = Constans.CompareMapping.Inprocess;
                        //Validate updated production plan status with current existing
                        switch ((Constans.PRODUCTION_PLAN_STATUS)plan.StatusId)
                        {
                            case Constans.PRODUCTION_PLAN_STATUS.Production:
                            case Constans.PRODUCTION_PLAN_STATUS.Preparatory:
                            case Constans.PRODUCTION_PLAN_STATUS.Changeover:
                            case Constans.PRODUCTION_PLAN_STATUS.CleaningAndSanitation:
                            case Constans.PRODUCTION_PLAN_STATUS.MealTeaBreak:
                                if (plan.PlanFinish.Value < timeNow.AddHours(timeBuffer))
                                    plan.CompareResult = Constans.CompareMapping.InvalidDateTime;
                                else if (activeProductionPlanOutput != null && activeProductionPlanOutput.ContainsKey(plan.PlanId))
                                    if (activeProductionPlanOutput[plan.PlanId] > plan.Target + targetBuffer)
                                        plan.CompareResult = Constans.CompareMapping.InvalidTarget;
                                break;
                            case Constans.PRODUCTION_PLAN_STATUS.New:
                            case Constans.PRODUCTION_PLAN_STATUS.Hold:
                            case Constans.PRODUCTION_PLAN_STATUS.Cancel:
                                break;
                            case Constans.PRODUCTION_PLAN_STATUS.Finished:
                                plan.CompareResult = Constans.CompareMapping.PlanFinished;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        plan.CompareResult = Constans.CompareMapping.NEW;
                    }
                }
                else
                {
                    plan.CompareResult = Constans.CompareMapping.NoProduct;
                }
            }
            return import;
        }
        public async Task<int> planStatus(string planId)
        {
            var plan = await _productionPlanRepository.WhereAsync(x => x.PlanId == planId);
            return plan.Select(x => x.StatusId).FirstOrDefault().Value;
        }

        public int ProductCodeToId(string Code, IDictionary<string, int> productDict)
        {
            int productCode;
            if (productDict.TryGetValue(Code, out productCode))
                return productCode;
            else
                return 0;
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
            int offsetTop = ExcelMapping.OFFSET_TOP_ROW;
            int offsetBottom = ExcelMapping.OFFSET_BOTTOM_ROW;
            for (int i = offsetTop; i <= totalRows - offsetBottom; i++)
            {
                ProductionPlanModel data = new ProductionPlanModel();
                data.PlanId = oSheet.Cells[i, ExcelMapping.PLAN].CellValToString();
                data.Route = oSheet.Cells[i, ExcelMapping.ROUTE].CellValToString();
                data.ProductCode = oSheet.Cells[i, ExcelMapping.PRODUCT].CellValToString();
                data.Target = oSheet.Cells[i, ExcelMapping.TARGET].CellValToInt();
                data.UnitName = oSheet.Cells[i, ExcelMapping.UNIT].CellValToString();
                data.PlanStart = oSheet.Cells[i, ExcelMapping.PLANSTART].CellValToDateTimeNull();
                data.PlanFinish = oSheet.Cells[i, ExcelMapping.PLANFINISH].CellValToDateTimeNull();
                listImport.Add(data);
            }
            return listImport;
        }

        public string GetProductionPlanKey(string id)
        {
            return $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{id}";
        }

        private string GetMachineKey(int machineId)
        {
            return $"{Constans.RedisKey.MACHINE}:{machineId}";
        }

        public async Task<List<int>> GetCachedActiveProcessRoutes(string id)
        {
            return (await _responseCacheService.GetAsTypeAsync<List<int>>(GetProductionPlanKey(id))) ?? new List<int>();
        }

        public async Task SetCachedActiveProcessRoute(string id, int[] routeIds)
        {
            await _responseCacheService.SetAsync(GetProductionPlanKey(id), routeIds);
        }

        public async Task AddCachedActiveProcessRoute(string id, int routeId)
        {
            var cachedProductionPlanRoutes = (await GetCachedActiveProcessRoutes(id)).ToList();
            cachedProductionPlanRoutes.Add(routeId);
            await SetCachedActiveProcessRoute(id, cachedProductionPlanRoutes.Distinct().ToArray());
        }

        public async Task RemoveCachedActiveProcessRoutes(string productionPlanKey)
        {
            await _responseCacheService.SetAsync(productionPlanKey, null);
        }

        public async Task<ActiveProductionPlanModel> Start(ProductionPlanModel model)
        {
            if (!model.RouteId.HasValue)
            {
                throw new Exception(ErrorMessages.PRODUCTION_PLAN.CANNOT_START_ROUTE_EMPTY);
            }

            var now = DateTime.Now;
            var masterData = await _masterDataService.GetData();
            if (masterData.Routes[model.RouteId.Value] == null)
            {
                throw new Exception(ErrorMessages.PRODUCTION_PLAN.CANNOT_ROUTE_INVALID);
            }

            var output = (await _activeProductionPlanService.GetCached(model.PlanId)) ?? new ActiveProductionPlanModel
            {
                ProductionPlanId = model.PlanId,
            };

            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == model.PlanId);
            if (dbModel.StatusId == (int)Constans.PRODUCTION_PLAN_STATUS.Production)
            {
                if (output.ActiveProcesses[model.RouteId.Value] != null)
                    throw new Exception(ErrorMessages.PRODUCTION_PLAN.PLAN_STARTED);
            }

            dbModel.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production;
            dbModel.PlanStart = now;
            dbModel.ActualStart = now;
            dbModel.UpdatedAt = now;
            dbModel.UpdatedBy = CurrentUser.UserId;
            _productionPlanRepository.Edit(dbModel);

            ValidateMachineLoss(masterData.Routes[model.RouteId.Value].MachineList);

            _recordActiveProductionPlanRepository.Add(new RecordActiveProductionPlan
            {
                CreatedBy = CurrentUser.UserId,
                ProductionPlanPlanId = model.PlanId,
                RouteId = (int)model.RouteId,
                Start = now,
                StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Production,
                Target = !model.Target.HasValue ? (int)dbModel.Target : CalculateTarget(model),
            });

            output.ActiveProcesses[model.RouteId.Value] = new ActiveProcessModel
            {
                ProductionPlanId = model.PlanId,
                ProductId = model.ProductId,
                Route = new ActiveRouteModel
                {
                    Id = model.RouteId.Value,
                    MachineList = masterData.Routes[model.RouteId.Value].MachineList,
                }
            };

            await _machineService.BulkCacheMachines(model.PlanId, model.RouteId.Value, output.ActiveProcesses[model.RouteId.Value].Route.MachineList);
            await _activeProductionPlanService.SetCached(output);
            await _unitOfWork.CommitAsync();
            return output;
        }

        private void ValidateMachineLoss(Dictionary<int, MachineModel> machineList)
        {
            //todo
            // validate mchine loss finish time stamp

            //check if machine of current route has finish = null
            //Yes-> stamp finish = now

            throw new NotImplementedException();
        }

        private int CalculateTarget(ProductionPlanModel model)
        {
            // todo calculate target, need final business logic
            return (int)model.Target;
        }

        public async Task Resume(string id, int routeId)
        {
            var masterData = await _masterDataService.GetData();
            ValidateMachineLoss(masterData.Routes[routeId].MachineList);
            //todo

            await _unitOfWork.CommitAsync();

        }

        public async Task Pause(string id, int routeId)
        {

            var now = DateTime.Now;
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == id);
            // todo
            // If productionstatus = Production && loss record with same planId and routeId or machineId exist, which has finsih != null  

            _recordManufacturingLossRepository.Add(new RecordManufacturingLoss
            {
                Guid = Guid.NewGuid().ToString(),
                CreatedBy = CurrentUser.UserId,
                LossLevel3Id = Constans.DEFAULT_LOSS_LV3,
                RouteId = routeId,
            });

            var activeProductionPlan = await _activeProductionPlanService.GetCached(id);
            if (activeProductionPlan != null)
            {
                var activeProcess = activeProductionPlan.ActiveProcesses[routeId];
                if (activeProcess == null)
                    throw new Exception($"Route {routeId} is not active in PlanId {id}.");

                foreach (var machine in activeProcess.Route.MachineList)
                {
                    machine.Value.RouteList.Remove(routeId);
                    // todo
                    // if machine is no long in any route -> insert record_manufactoring_loss
                    await _machineService.RemoveCached(machine.Key, null);
                }
                activeProductionPlan.ActiveProcesses.Remove(activeProcess.Route.Id);
            }
            await _unitOfWork.CommitAsync();

        }

        public async Task Finish(ProductionPlanModel model)
        {

            var now = DateTime.Now;
            var masterData = await _masterDataService.GetData();
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == model.PlanId);
            await _recordManufacturingLossService.Create(new RecordManufacturingLossModel
            {
                Guid = Guid.NewGuid().ToString(),
                CreatedBy = CurrentUser.UserId,
                LossLevel3Id = Constans.DEFAULT_LOSS_LV3,

            });
            dbModel.UpdatedAt = now;
            dbModel.UpdatedBy = CurrentUser.UserId;
            _productionPlanRepository.Edit(dbModel);

            var activeProductionPlan = await _activeProductionPlanService.GetCached(model.PlanId);

            if (activeProductionPlan != null)
            {
                var activeProcess = activeProductionPlan.ActiveProcesses[(int)model.RouteId];
                if (activeProcess == null)
                    throw new Exception($"Route {model.RouteId} is not active in PlanId {model.PlanId}. ");


                foreach (var machine in activeProcess.Route.MachineList)
                {
                    await _machineService.RemoveCached(machine.Key, null);
                }
                activeProductionPlan.ActiveProcesses.Remove(activeProcess.Route.Id);
                if (activeProductionPlan.ActiveProcesses.Count() == 0)
                    await _activeProductionPlanService.RemoveCached(activeProductionPlan.ProductionPlanId);
            }
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
                            ProductGroupId = x.Product.ProductGroupId,
                            ProductGroup = x.Product.ProductGroup.Name,
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
                            Product = new ProductModel
                            {
                                Id = x.Product.Id,
                                Code = x.Product.Code,
                                Description = x.Product.Description,
                                BriteItemPerUpcitem = x.Product.BriteItemPerUpcitem,
                                ProductFamily_Id = x.Product.ProductFamilyId,
                                ProductFamily = x.Product.ProductFamily.Description,
                                ProductGroup_Id = x.Product.ProductGroupId,
                                ProductGroup = x.Product.ProductGroup.Name,
                                ProductType_Id = x.Product.ProductTypeId,
                                ProductType = x.Product.ProductType.Description,
                                PackingMedium = x.Product.PackingMedium,
                                NetWeight = x.Product.NetWeight,
                                Igweight = x.Product.Igweight,
                                Pmweight = x.Product.Pmweight,
                                WeightPerUom = x.Product.WeightPerUom,
                                IsActive = x.Product.IsActive,
                                IsDelete = x.Product.IsDelete,
                                CreatedAt = x.Product.CreatedAt,
                                CreatedBy = x.Product.CreatedBy,
                                UpdatedAt = x.Product.UpdatedAt,
                                UpdatedBy = x.Product.UpdatedBy
                            }
                        }).FirstOrDefaultAsync();
            return output;
        }

        public async Task<ActiveProductionPlanModel> TakeAction(string id)
        {
            var output = await _activeProductionPlanService.GetCached(id);

            var alertList = output.Alerts.Where(x => x.StatusId == (int)Constans.AlertStatus.New);
            foreach (var item in alertList)
            {
                item.StatusId = (int)Constans.AlertStatus.Processing;
            }
            await _activeProductionPlanService.SetCached(output);
            return output;
        }

        public async Task<ActiveProductionPlanModel> UpdateByMachine(int machineId, int statusId)
        {
            var cachedMachine = await _machineService.GetCached(machineId);
            var masterData = await _masterDataService.GetData();
            var machine = masterData.Machines[machineId];
            ActiveProductionPlanModel output = null;
            //ActiveProcessModel activeProcess = null;
            // If Production Plan doesn't start but machine just start to send status
            if (cachedMachine == null)
            {
                cachedMachine = new ActiveMachineModel
                {
                    Id = machine.Id,
                    StatusId = statusId
                };
                await _machineService.SetCached(machineId, cachedMachine);
            }

            //if machine is apart of production plan
            if (!string.IsNullOrEmpty(cachedMachine.ProductionPlanId) && cachedMachine.RouteIds != null)
            {
                output = await _activeProductionPlanService.GetCached(cachedMachine.ProductionPlanId);
                foreach (var routeId in cachedMachine.RouteIds)
                {
                    output.ActiveProcesses[routeId].Route.MachineList[machineId].StatusId = statusId;
                    var alert = new AlertModel
                    {
                        StatusId = (int)Constans.AlertStatus.New,
                        ItemStatusId = statusId,
                        CreatedAt = DateTime.Now,
                        Id = Guid.NewGuid(),
                        ItemId = machineId,
                        ItemType = (int)Constans.AlertType.MACHINE
                    };
                    output.Alerts.Add(alert);
                    await _recordManufacturingLossService.Create(new RecordManufacturingLossModel
                    {
                        CreatedBy = CurrentUser.UserId,
                        Guid = alert.Id.ToString(),
                        IsAuto = false,
                        LossLevel3Id = Constans.DEFAULT_LOSS_LV3,
                        MachineId = machineId,
                        ProductionPlanId = cachedMachine.ProductionPlanId,
                        StartedAt = DateTime.Now,
                    });
                }
                await _activeProductionPlanService.SetCached(output);

            }

            return output;
        }
    }
}
