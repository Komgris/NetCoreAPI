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
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.Internal;
using System.Globalization;

namespace CIM.BusinessLogic.Services
{
    public class ProductionPlanService : BaseService, IProductionPlanService
    {
        private IResponseCacheService _responseCacheService;
        private IMasterDataService _masterDataService;
        private IProductionPlanRepository _productionPlanRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private IActiveProductionPlanService _activeProductionPlanService;
        private IReportService _reportService;
        private IDirectSqlRepository _directSqlRepository;

        public ProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IDirectSqlRepository directSqlRepository,
            IUnitOfWorkCIM unitOfWork,
            IProductionPlanRepository productionPlanRepository,
            IActiveProductionPlanService activeProductionPlanService,
            IReportService reportService
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _directSqlRepository = directSqlRepository;
            _productionPlanRepository = productionPlanRepository;
            _unitOfWork = unitOfWork;
            _activeProductionPlanService = activeProductionPlanService;
            _reportService = reportService;
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
                    await Update(plan);
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
            var db_model = MapperHelper.AsModel(model, new ProductionPlan(), new[] { "Route", "Product", "Status", "Unit" });

            db_model.PlanId = model.PlanId.Trim();
            db_model.UnitId = model.Unit;
            db_model.UpdatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.UpdatedAt = DateTime.Now;
            _productionPlanRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();

            //recalc target per active-route
            var dbmodel = _productionPlanRepository.Where(x => x.PlanId == model.PlanId && x.StatusId == 2).FirstOrDefault();
            if (dbmodel != null)
            {
                var paramsList = new Dictionary<string, object>() {
                            {"@planid", model.PlanId },
                            {"@user", CurrentUser.UserId}
                        };
                _directSqlRepository.ExecuteSPNonQuery("sp_Process_Production_RecalcTarget", paramsList);
            }
        }

        public ProductionPlanModel CreatePlan(ProductionPlanModel model)
        {
            var db_model = MapperHelper.AsModel(model, new ProductionPlan(), new[] { "Route", "Unit" });
            db_model.PlanId = model.PlanId.Trim();
            db_model.UnitId = model.Unit;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.CreatedAt = DateTime.Now;
            db_model.IsActive = true;
            db_model.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.New;
            _productionPlanRepository.Add(db_model);
            return (MapperHelper.AsModel(db_model, new ProductionPlanModel()));
        }

        public async Task Delete(string id)
        {
            var existingItem = _productionPlanRepository.Where(x => x.PlanId == id).ToList().FirstOrDefault();
            existingItem.IsActive = false;
            existingItem.UpdatedAt = DateTime.Now;
            existingItem.UpdatedBy = CurrentUser.UserId;
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

        public async Task<List<ProductionPlanModel>> ReadImport(string path)
        {
            FileInfo excel = new FileInfo(path);
            using (var package = new ExcelPackage(excel))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.First();
                List<ProductionPlanModel> intList = await ConvertImportToList(worksheet);
                return intList;
            }
        }

        public async Task<List<ProductionPlanModel>> ConvertImportToList(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            List<ProductionPlanModel> listImport = new List<ProductionPlanModel>();
            int offsetTop = ExcelMapping.OFFSET_TOP_ROW;
            int offsetBottom = ExcelMapping.OFFSET_BOTTOM_ROW;
            for (int i = offsetTop; i <= totalRows - offsetBottom; i++)
            {
                ProductionPlanModel data = new ProductionPlanModel();             
                data.Line = oSheet.Cells[i, ExcelMapping.LINE].CellValToString();
                data.Wbrt = oSheet.Cells[i, ExcelMapping.WBRT].CellValToString();
                data.RouteGuideLine = oSheet.Cells[i, ExcelMapping.ROUTE_GUILDLINE].CellValToString();
                data.ProductCode = oSheet.Cells[i, ExcelMapping.PRODUCT].CellValToString();
                data.Country = oSheet.Cells[i, ExcelMapping.COUNTRY].CellValToString();
                data.RawMaterial = oSheet.Cells[i, ExcelMapping.RAWMATERIAL].CellValToString();
                data.Ingredient = oSheet.Cells[i, ExcelMapping.INGREDIENT].CellValToString();
                data.Brix = oSheet.Cells[i, ExcelMapping.BRIX].CellValToString();
                data.Acid = oSheet.Cells[i, ExcelMapping.ACID].CellValToString();
                data.Ph = oSheet.Cells[i, ExcelMapping.PH].CellValToString();
                data.Weight = oSheet.Cells[i, ExcelMapping.WEIGHT].CellValToString();
                data.Pm = oSheet.Cells[i, ExcelMapping.PM].CellValToString();
                data.TotalLine = oSheet.Cells[i, ExcelMapping.TOTALLINE].CellValToInt();
                data.Target = oSheet.Cells[i, ExcelMapping.TARGET].CellValToInt();
                data.UnitName = oSheet.Cells[i, ExcelMapping.UNIT].CellValToString();
                data.PlanStart = oSheet.Cells[i, ExcelMapping.PLANSTART].CellValToDateTimeNull();
                data.PlanFinish = oSheet.Cells[i, ExcelMapping.PLANFINISH].CellValToDateTimeNull();
                data.Note = oSheet.Cells[i, ExcelMapping.NOTE].CellValToString();
                data.PlanId = $"{DateTime.Now.ToString("yyMMddHHmm")}-{data.ProductCode}-{i.ToString("00")}" ;
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

        public async Task<ProductionPlanOverviewModel> Load(string id,int routeId)
        {
            var productionPlan = await _productionPlanRepository.Load(id, routeId);
            var activeProductionPlan = await _activeProductionPlanService.GetCached(id);
            var route = new ActiveProcessModel();
            if (activeProductionPlan != null && activeProductionPlan.ActiveProcesses.ContainsKey(routeId))
            {
                route = activeProductionPlan?.ActiveProcesses[routeId];
            } 
            else
            {
                route = new ActiveProcessModel
                {
                    Route = new ActiveRouteModel { Id = routeId },
                    Status = Constans.PRODUCTION_PLAN_STATUS.New
                };
            }
            return new ProductionPlanOverviewModel
            {
                ProductionPlan = productionPlan,
                Route = route
            };            
        }

        public async Task<ProductionPlanModel> Get(string planId)
        {
            var output = await _productionPlanRepository.Where( x=>x.PlanId == planId).Select(
                        x => new ProductionPlanModel
                        {
                            PlanId = x.PlanId,
                            ProductId = x.ProductId,
                            RouteGuideLine = x.RouteGuideLine,
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
                                PID = x.Product.PID,
                                Description = x.Product.Description,
                                BriteItemPerUPCItem = x.Product.BriteItemPerUPCItem,
                                ProductFamilyId = x.Product.ProductFamilyId,
                                ProductFamily = x.Product.ProductFamily.Description,
                                ProductGroupId = x.Product.ProductGroupId,
                                ProductGroup = x.Product.ProductGroup.Name,
                                ProductTypeId = x.Product.ProductTypeId,
                                ProductType = x.Product.ProductType.Description,
                                PackingMedium = x.Product.PackingMedium,
                                NetWeight = x.Product.NetWeight,
                                IGWeight = x.Product.IGWeight,
                                PMWeight = x.Product.PMWeight,
                                WeightPerUOM = x.Product.WeightPerUOM,
                                Image = x.Product.Image,
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

        public async Task<ActiveProductionPlanModel> TakeAction(string id, int routeId)
        {
            var output = await _activeProductionPlanService.GetCached(id);

            var alertList = output.ActiveProcesses[routeId].Alerts.Where(x => x.StatusId == (int)Constans.AlertStatus.New);
            foreach (var item in alertList)
            {
                item.StatusId = (int)Constans.AlertStatus.Processing;
            }
            await _activeProductionPlanService.SetCached(output);
            return output;
        }

        public FilterLoadProductionPlanListModel FilterLoadProductionPlan(int? productId, int? routeId, int? statusId, string planId)
        {
            var output = _productionPlanRepository.FilterLoadProductionPlan(productId, routeId, statusId, planId);
            return output;
        }

        public async Task<List<ProductionPlanListModel>> ListByMonth(int month, int year, string statusIds)
        {
            var output = await _productionPlanRepository.ListByMonth(month, year, statusIds);
            return output;
        }

        public async Task<PagingModel<ProductionPlanListModel>> ListByDate(DateTime date, int page, int howmany, string statusIds)
        {
            var output = await _productionPlanRepository.ListByDate(date, page, howmany, statusIds);
            return output;
        }

        public async Task<PagingModel<ProductionOutputModel>> ListOutput(int page, int howmany, string keyword, bool isActive, string statusIds)
        {
            var output = await _productionPlanRepository.ListOutput(page, howmany, keyword, isActive, statusIds);
            return output;
        }

        public async Task<List<ProductionOutputModel>> ListOutputByMonth(int month, int year)
        {
            var output = await _productionPlanRepository.ListOutputByMonth(month, year);
            return output;
        }

        public async Task<PagingModel<ProductionOutputModel>> ListOutputByDate(DateTime date, int page, int howmany)
        {
            var output = await _productionPlanRepository.ListOutputByDate(date, page, howmany);
            return output;
        }
    }
}
