﻿using CIM.BusinessLogic.Interfaces;
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
using System.Data;

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

        public ProductionPlanService(
            IResponseCacheService responseCacheService,
            IMasterDataService masterDataService,
            IUnitOfWorkCIM unitOfWork,
            IProductionPlanRepository productionPlanRepository,
            IProductRepository productRepository,
            IMachineService machineService,
            IActiveProductionPlanService activeProductionPlanService,
            IRecordManufacturingLossService recordManufacturingLossService
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
                    var model = await Create(plan);
                    db_list.Add(model);
                }
            }
            return db_list;
        }


        public async Task<ProductionPlanModel> Create(ProductionPlanModel model)
        {
            var db_model = MapperHelper.AsModel(model, new ProductionPlan(), new[] { "Route", "Unit" });
            db_model.UnitId = model.Unit;
            db_model.CreatedBy = CurrentUser.UserId;
            db_model.CreatedAt = DateTime.Now;
            db_model.IsActive = true;
            db_model.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.New;
            _productionPlanRepository.Add(db_model);
            await _unitOfWork.CommitAsync();
            return (MapperHelper.AsModel(db_model, new ProductionPlanModel()));
        }

        public async Task Update(ProductionPlanModel model)
        {
            var db_model = MapperHelper.AsModel(model, new ProductionPlan(), new[] { "Route", "Product", "Status", "Unit" });
            db_model.UnitId = model.Unit;
            db_model.UpdatedBy = CurrentUser.UserId;
            db_model.IsActive = true;
            db_model.UpdatedAt = DateTime.Now;
            _productionPlanRepository.Edit(db_model);
            await _unitOfWork.CommitAsync();
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
            var productCodeToId = masterData.Dictionary.ProductsByCode;
            var productionPlanOutput = new ReportService().GetActiveProductionPlanOutput()?
                                            .AsEnumerable()
                                            .ToDictionary<DataRow, string,int>(row=>row.Field<string>(0),r=>r.Field<int>(1));
            DateTime timeNow = DateTime.Now;

            foreach (var plan in import)
            {
                plan.ProductId = ProductCodeToId(plan.ProductCode, productCodeToId);
                if (productDict.ContainsKey(plan.ProductId))
                {
                    if (productionPlanDict.ContainsKey(plan.PlanId))
                    {
                        plan.StatusId = productionPlanDict[plan.PlanId].StatusId;
                        plan.CompareResult = "Inprocess";
                        switch (plan.StatusId)
                        {
                            case (int)Constans.PRODUCTION_PLAN_STATUS.Production:
                            case (int)Constans.PRODUCTION_PLAN_STATUS.Preparatory:
                            case (int)Constans.PRODUCTION_PLAN_STATUS.Changeover:
                            case (int)Constans.PRODUCTION_PLAN_STATUS.CleaningAndSanitation:
                            case (int)Constans.PRODUCTION_PLAN_STATUS.MealTeaBreak:
                                if (plan.PlanFinish.Value < timeNow.AddHours(6))
                                    plan.CompareResult = "Imported finished date time must be further then now + 6h";
                                else if (productionPlanOutput != null && productionPlanOutput.ContainsKey(plan.PlanId))
                                       if(productionPlanOutput[plan.PlanId] > plan.Target + 100)
                                        plan.CompareResult = "Imported target is lower then current target";
                                break;
                            case (int)Constans.PRODUCTION_PLAN_STATUS.New:
                            case (int)Constans.PRODUCTION_PLAN_STATUS.Hold:
                            case (int)Constans.PRODUCTION_PLAN_STATUS.Cancel:
                                // Do some different stuff
                                break;
                            case (int)Constans.PRODUCTION_PLAN_STATUS.Finished:
                                plan.CompareResult = "Plan Finished";
                                break;
                            default:
                                plan.CompareResult = "";
                                    break;
                        }
                    }
                    else
                    {
                        plan.CompareResult = "NEW";
                    }
                }
                else
                {
                    plan.CompareResult = "No Product ID";
                }              
            }
            return import;
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
            for (int i = 5; i <= totalRows - 2; i++)
            {
                int _target;
                ProductionPlanModel data = new ProductionPlanModel();

                
                data.PlanId = (oSheet.Cells[i, 3].Value ?? string.Empty).ToString();
                data.Route = (oSheet.Cells[i, 4].Value ?? string.Empty).ToString();
                data.ProductCode = (oSheet.Cells[i, 5].Value ?? string.Empty).ToString();
                int.TryParse((oSheet.Cells[i, 15].Value ?? string.Empty).ToString(), out _target);
                data.Target = _target;
                data.UnitName = (oSheet.Cells[i, 16].Value ?? string.Empty).ToString();
                data.PlanStart = Convert.ToDateTime(oSheet.Cells[i, 17].Value ?? string.Empty);
                data.PlanFinish = Convert.ToDateTime(oSheet.Cells[i, 18].Value ?? string.Empty);
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

        public async Task Stop(string id, int[] routeIds)
        {

            var now = DateTime.Now;
            var masterData = await _masterDataService.GetData();
            var dbModel = await _productionPlanRepository.FirstOrDefaultAsync(x => x.PlanId == id);

            dbModel.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.Finished;
            dbModel.UpdatedAt = now;
            dbModel.UpdatedBy = CurrentUser.UserId;
            _productionPlanRepository.Edit(dbModel);

            var activeProductionPlan = await _activeProductionPlanService.GetCached(id);

            if (activeProductionPlan != null)
            {
                foreach (var activeProcess in activeProductionPlan.ActiveProcesses)
                {
                    if (routeIds.Length == 0 || routeIds.Contains(activeProcess.Key))
                    {
                        foreach (var machine in activeProcess.Value.Route.MachineList)
                        {
                            await _machineService.RemoveCached(machine.Key, null);
                        }
                        activeProductionPlan.ActiveProcesses.Remove(activeProcess.Key);
                    }
                }
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
