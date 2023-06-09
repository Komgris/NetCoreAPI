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
        private IMachineService _machineService;
        private IReportService _reportService;
        private IDirectSqlRepository _directSqlRepository;

        public ProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IDirectSqlRepository directSqlRepository,
            IUnitOfWorkCIM unitOfWork,
            IProductionPlanRepository productionPlanRepository,
            IActiveProductionPlanService activeProductionPlanService,
            IReportService reportService,
            IMachineService machineService
            )
        {
            _responseCacheService = responseCacheService;
            _masterDataService = masterDataService;
            _directSqlRepository = directSqlRepository;
            _productionPlanRepository = productionPlanRepository;
            _unitOfWork = unitOfWork;
            _activeProductionPlanService = activeProductionPlanService;
            _reportService = reportService;
            _machineService = machineService;
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

        public async Task<PagingModel<ProductionPlan3MModel>> List(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive, string statusIds, int? machineId)
        {
            var output = await _productionPlanRepository.ListAsPaging(page, howmany, keyword, productId, routeId, isActive, statusIds, machineId);
            return output;
        }


        public async Task<List<ProductionPlan3MModel>> CheckDuplicate(List<ProductionPlan3MModel> import)
        {
            List<ProductionPlan3MModel> db_list = new List<ProductionPlan3MModel>();
            var masterData = await _masterDataService.GetData();
            var planList = await _productionPlanRepository.AllAsync();
            foreach (var plan in import)
            {
                var dbModel = planList.FirstOrDefault(x => x.PlanId == plan.PlanId);
                if (dbModel != null)
                {
                    await Update3M(plan, dbModel);
                }
                else
                {
                    var model = CreatePlan3M(plan);
                    db_list.Add(model);
                }
            }
            await _unitOfWork.CommitAsync();
            return db_list;
        }

        public async Task<List<ProductionMasterShowModel>> DeletePlans(List<ProductionMasterShowModel> data)
        {
            List<ProductionMasterShowModel> db_list = new List<ProductionMasterShowModel>();
            var masterData = await _masterDataService.GetData();
            var planList = await _productionPlanRepository.AllAsync();
            foreach (var plan in data)
            {
                var dbModel = planList.FirstOrDefault(x => x.PlanId == plan.PlanId);
                if (dbModel != null)
                {
                    await Delete(plan.PlanId);
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

        public async Task Update3M(ProductionPlan3MModel model, ProductionPlan db_model)
        {
            db_model.PlanId = model.PlanId.Trim();
            db_model.ProductId = model.ProductId;
            db_model.MachineId = model.MachineId;
            db_model.ShopNo = model.ShopNo;
            db_model.Target = model.Target;
            db_model.Sequence = model.Sequence;
            db_model.PlanStart = model.PlanStart;
            db_model.PlanFinish = model.PlanFinish;
            db_model.Sequence = model.Sequence;
            db_model.Standard = model.Standard;
            db_model.UpdatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.UpdatedAt = DateTime.Now;
            //db_model.Id = null;
            _productionPlanRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
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

        public ProductionPlan3MModel CreatePlan3M(ProductionPlan3MModel model)
        {
            var db_model = MapperHelper.AsModel(model, new ProductionPlan());
            db_model.PlanId = model.PlanId.Trim();
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.CreatedAt = DateTime.Now;
            db_model.IsActive = true;
            db_model.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.New;
            _productionPlanRepository.Add(db_model);
            return (MapperHelper.AsModel(db_model, new ProductionPlan3MModel()));
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
            var routeDict = masterData.Dictionary.RouteByName;
            var productCodeToIds = masterData.Dictionary.ProductsByCode;
            var activeProductionPlanOutput = _reportService.GetActiveProductionPlanOutput();
            var timeBuffer = (int)Constans.ProductionPlanBuffer.HOUR_BUFFER;
            var targetBuffer = (int)Constans.ProductionPlanBuffer.TARGET_BUFFER;

            DateTime timeNow = DateTime.Now;

            foreach (var plan in import)
            {
                plan.RouteId = RouteGuideLineToId(plan.RouteGuideLine.Trim(), routeDict);
                plan.ProductId = ProductCodeToId(plan.ProductCode, productCodeToIds);
                if (plan.ProductId != 0)
                {
                    if (plan.PlanStart == null || plan.PlanFinish == null)
                    {
                        plan.CompareResult = Constans.CompareMapping.InvalidDateTime;
                    }
                    else
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
                }
                else
                {
                    plan.CompareResult = Constans.CompareMapping.NoProduct;
                }
            }
            return import;
        }
        public async Task<List<ProductionPlan3MModel>> validatePlan(List<ProductionPlan3MModel> data)
        {
            var masterData = await _masterDataService.GetData();
            var planList = await _productionPlanRepository.AllAsync();
            var productCodeToIds = masterData.Dictionary.ProductsByCode;
            var machineCodeToIds = masterData.Dictionary.MachineByCode;

            DateTime timeNow = DateTime.Now;

            foreach (var plan in data)
            {
                plan.PlanId = $"{plan.MachineCode}_{plan.Sequence}_{DateTime.Now.ToString("MM")}_{DateTime.Now.ToString("yy")}";
                plan.MachineId = MachineCodeToId(plan.MachineCode, machineCodeToIds);
                plan.ProductId = ProductCodeToId(plan.ProductCode, productCodeToIds);
                if (plan.MachineId != 0)
                {
                    if (plan.ProductId != 0)
                    {
                        if (plan.PlanStart == null || plan.PlanFinish == null)
                        {
                            plan.CompareResult = Constans.CompareMapping.InvalidDateTime;
                        }
                        else if(plan.Standard == null)
                        {
                            plan.CompareResult = Constans.CompareMapping.InvalidStandardRate;
                        }
                        else
                        {
                            if (planList.FirstOrDefault(x => x.PlanId == plan.PlanId) != null)
                            {
                                plan.StatusId = await planStatus(plan.PlanId);
                                plan.CompareResult = Constans.CompareMapping.Inprocess;
                                switch ((Constans.PRODUCTION_PLAN_STATUS)plan.StatusId)
                                {
                                    case Constans.PRODUCTION_PLAN_STATUS.Production:
                                        plan.CompareResult = Constans.CompareMapping.Production;
                                        break;
                                    case Constans.PRODUCTION_PLAN_STATUS.Preparatory:
                                        plan.CompareResult = Constans.CompareMapping.Preparatory;
                                        break;
                                    case Constans.PRODUCTION_PLAN_STATUS.New:
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
                    }
                    else
                    {
                        plan.CompareResult = Constans.CompareMapping.NoProduct;
                    }
                }
                else
                {
                    plan.CompareResult = Constans.CompareMapping.NoMachine;
                }
            }
            return data;
        }
        public async Task<int> planStatus(string planId)
        {
            var plan = await _productionPlanRepository.WhereAsync(x => x.PlanId == planId);
            return plan.Select(x => x.StatusId).FirstOrDefault().Value;
        }

        public int? RouteGuideLineToId(string routeGuideLine, IDictionary<string, int> routeDict)
        {
            int routeId;
            if (routeDict.TryGetValue(routeGuideLine, out routeId))
                return routeId;
            else
                return null;
        }
        public int MachineCodeToId(string Code,IDictionary<string, int> machineDict)
        {
            int machineId;
            if (machineDict.TryGetValue(Code, out machineId))
                return machineId;
            else
                return 0;
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
                data.PlanId = $"{DateTime.Now.ToString("yyMMddHHmm")}-{data.ProductCode}-{i.ToString("00")}";
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

        public async Task<ProductionPlanOverviewModel> Load(string id, int routeId)
        {
            var productionPlan = await _productionPlanRepository.Load(id, routeId);
            var activeProductionPlan = await _activeProductionPlanService.GetCached(id);
            var route = new ActiveProcessModel();
            if (activeProductionPlan != null && activeProductionPlan.ActiveProcesses.ContainsKey(routeId))
            {
                route = activeProductionPlan.ActiveProcesses[routeId];
            }
            else
            {
                route = new ActiveProcessModel
                {
                    Status = Constans.PRODUCTION_PLAN_STATUS.New
                };
            }
            return new ProductionPlanOverviewModel
            {
                ProductionPlan = productionPlan,
                Route = route
            };
        }

        public async Task<ProductionPlanOverviewModel> Load3M(string planId)
        {
            var productionPlan = await _productionPlanRepository.Load3M(planId);
            //var activeProductionPlan = await _activeProductionPlanService.GetCached3M(id);
            //if (activeProductionPlan != null && activeProductionPlan.ActiveProcesses.ContainsKey(machineId))
            //{
            //    route = activeProductionPlan?.ActiveProcesses[machineId];
            //}
            //else
            //{
            //    route = new ActiveProcess3MModel
            //    {
            //        //Route = new ActiveRouteModel { Id = routeId },
            //        Status = Constans.PRODUCTION_PLAN_STATUS.New
            //    };
            //}
            return new ProductionPlanOverviewModel
            {
                ProductionPlan = productionPlan,
                //Route = route
            };
        }

        public async Task<ProductionPlanModel> Get(string planId)
        {
            var output = await _productionPlanRepository.Where(x => x.PlanId == planId).Select(
                        x => new ProductionPlanModel
                        {
                            PlanId = x.PlanId,
                            ProductId = x.ProductId,
                            //Target = x.Target,
                            Unit = x.UnitId,
                            PlanStart = x.PlanStart,
                            PlanFinish = x.PlanFinish,
                            ActualStart = x.ActualStart,
                            ActualFinish = x.ActualFinish,
                            StatusId = x.StatusId,
                            IsActive = x.IsActive,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                            UpdatedBy = x.UpdatedBy,
                        }).FirstOrDefaultAsync();
            return output;
        }

        public async Task<ProductionPlan3MModel> Get3M(string planId)
        {
            var output = await _productionPlanRepository.Where(x => x.PlanId == planId).Select(
                        x => new ProductionPlan3MModel
                        {
                            PlanId = x.PlanId,
                            PlanStart = x.PlanStart,
                            PlanFinish = x.PlanFinish,
                            ActualStart = x.ActualStart,
                            ActualFinish = x.ActualFinish,
                            StatusId = x.StatusId,
                            IsActive = x.IsActive,
                            CreatedAt = x.CreatedAt,
                            CreatedBy = x.CreatedBy,
                            UpdatedAt = x.UpdatedAt,
                            UpdatedBy = x.UpdatedBy,
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

        public async Task<ActiveMachine3MModel> TakeAction3M(string id, int machineId)
        {
            var output = await _machineService.GetCached3M(machineId);            

            var alertList = output.Alerts.Where(x => x.StatusId == (int)Constans.AlertStatus.New);
            foreach (var item in alertList)
            {
                item.StatusId = (int)Constans.AlertStatus.Processing;
            }
            await _machineService.SetCached3M(output);
            return output;
        }

        public FilterLoadProductionPlanListModel FilterLoadProductionPlan(int? productId, int? routeId, int? statusId, string planId, int? processTypeId)
        {
            var output = _productionPlanRepository.FilterLoadProductionPlan(productId, routeId, statusId, planId, processTypeId);
            return output;
        }

        public async Task<List<ProductionPlanListModel>> ListByMonth(int month, int year, string statusIds)
        {
            var output = await _productionPlanRepository.ListByMonth(month, year, statusIds);
            return output;
        }

        public async Task<PagingModel<ProductionPlanListModel>> ListByDate(DateTime date, int page, int howmany, string statusIds, int? processTypeId)
        {
            var output = await _productionPlanRepository.ListByDate(date, page, howmany, statusIds, processTypeId);
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
