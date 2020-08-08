﻿using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RecordManufacturingLossService : BaseService, IRecordManufacturingLossService {
        private IResponseCacheService _responseCacheService;
        private IRecordManufacturingLossRepository _recordManufacturingLossRepository;
        private IMasterDataService _masterDataService;
        private IRecordProductionPlanWasteService _recordProductionPlanWasteService;
        private IRecordProductionPlanWasteRepository _recordProductionPlanWasteRepository;
        private IMachineService _machineService;
        private IMachineRepository _machineRepository;
        private IUnitOfWorkCIM _unitOfWork;
        public IConfiguration _config;
        private IMaterialRepository _materialRepository;
        private IProductMaterialRepository _productmaterialRepository;

        public RecordManufacturingLossService(
            IResponseCacheService responseCacheService,
            IRecordManufacturingLossRepository recordManufacturingLossRepository,
            IMasterDataService masterDataService,
            IRecordProductionPlanWasteService recordProductionPlanWasteService,
            IRecordProductionPlanWasteRepository recordProductionPlanWasteRepository,
            IMachineService machineService,
            IMachineRepository machineRepository,
            IUnitOfWorkCIM unitOfWork,
            IConfiguration config,
            IMaterialRepository materialRepository,
            IProductMaterialRepository productmaterialRepository
            )
        {
            _responseCacheService = responseCacheService;
            _recordManufacturingLossRepository = recordManufacturingLossRepository;
            _masterDataService = masterDataService;
            _recordProductionPlanWasteService = recordProductionPlanWasteService;
            _recordProductionPlanWasteRepository = recordProductionPlanWasteRepository;
            _machineService = machineService;
            _unitOfWork = unitOfWork;
            _machineRepository = machineRepository;
            _config = config;
            _materialRepository = materialRepository;
            _productmaterialRepository = productmaterialRepository;

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

        private async Task NewRecordManufacturingLoss(RecordManufacturingLossModel model, DateTime now, string guid)
        {
            var newDbModel = new RecordManufacturingLoss();
            newDbModel.Guid = guid;
            newDbModel.CreatedBy = CurrentUser.UserId;
            newDbModel.StartedAt = now;
            newDbModel.IsAuto = model.IsAuto;
            newDbModel.MachineId = model.MachineId;
            newDbModel.ProductionPlanId = model.ProductionPlanId;
            newDbModel.RouteId = model.RouteId;
            newDbModel.LossLevel3Id = model.LossLevelId;
            newDbModel.ComponentId = model.ComponentId > 0 ? model.ComponentId : null;
            newDbModel.Remark = model.Remark;
            newDbModel = await HandleWaste(newDbModel, model, now);
            _recordManufacturingLossRepository.Add(newDbModel);
        }

        public async Task<ActiveProductionPlanModel> Create(RecordManufacturingLossModel model)
        {
            var now = DateTime.Now;
            var guid = Guid.NewGuid();
            var activeProductionPlan = await GetCached(model.ProductionPlanId);

            var dbModel = await _recordManufacturingLossRepository.FirstOrDefaultAsync(x => x.MachineId == model.MachineId && x.EndAt.HasValue == false);
            // doesn't exist
            if (dbModel  == null)
            {
                await NewRecordManufacturingLoss(model, now, guid.ToString());
            }

            // already exist and and processing manually
            if (dbModel != null && model.IsAuto == false )
            {
                //End current loss
                dbModel.EndAt = now;
                dbModel.EndBy = CurrentUser.UserId;
                dbModel.Timespan = Convert.ToInt64((now - dbModel.StartedAt).TotalSeconds);
                dbModel.IsBreakdown = dbModel.Timespan >= 600;//10 minute
                if (dbModel.Timespan < 60 && dbModel.IsAuto)
                {
                    dbModel.LossLevel3Id = _config.GetValue<int>("DefaultSpeedLosslv3Id");
                    var sploss = activeProductionPlan.ActiveProcesses[model.RouteId].Alerts.FirstOrDefault(x => x.Id == Guid.Parse(dbModel.Guid));
                    //handle case alert is removed from redis
                    if (sploss != null)
                    {
                        sploss.LossLevel3Id = dbModel.LossLevel3Id;
                        sploss.StatusId = (int)Constans.AlertStatus.Edited;
                    }
                }
                _recordManufacturingLossRepository.Edit(dbModel);

                //Create new
                await NewRecordManufacturingLoss(model, now, guid.ToString());
            }

            await _unitOfWork.CommitAsync();
            var alert = new AlertModel
            {
                CreatedAt = now,
                ItemId = model.MachineId,
                ItemStatusId = (int)Constans.AlertStatus.Edited,
                ItemType = (int)Constans.AlertType.MACHINE,
                LossLevel3Id = model.LossLevelId,
                StatusId = Constans.MACHINE_STATUS.Stop,
                Id = guid
            };
            activeProductionPlan.ActiveProcesses[model.RouteId].Alerts.Add(alert);

            return await UpdateActiveProductionPlanMachine(model.RouteId, model.MachineId, Constans.MACHINE_STATUS.Stop, activeProductionPlan); 
        }

        public async Task<ActiveProductionPlanModel> End(RecordManufacturingLossModel model)
        {
            var dbModel = await _recordManufacturingLossRepository.FirstOrDefaultAsync(x => x.MachineId == model.MachineId && x.EndAt.HasValue == false);
            var activeProductionPlan = await GetCached(model.ProductionPlanId);
            var now = DateTime.Now;
            if (dbModel != null)
            {
                dbModel.EndAt = now;
                dbModel.EndBy = CurrentUser.UserId;
                dbModel.Timespan = Convert.ToInt64((now - dbModel.StartedAt).TotalSeconds);
                dbModel.IsBreakdown = dbModel.Timespan >= 600;//10 minute
                if (dbModel.Timespan < 60 && dbModel.IsAuto) {
                    dbModel.LossLevel3Id = _config.GetValue<int>("DefaultSpeedLosslv3Id");
                    var sploss = activeProductionPlan.ActiveProcesses[model.RouteId].Alerts.FirstOrDefault(x => x.Id == Guid.Parse(dbModel.Guid));
                    //handle case alert is removed from redis
                    if (sploss != null)
                    {
                        sploss.LossLevel3Id = dbModel.LossLevel3Id;
                        sploss.StatusId = (int)Constans.AlertStatus.Edited;
                    }
                }

                _recordManufacturingLossRepository.Edit(dbModel);
                await _unitOfWork.CommitAsync();

                var alert = activeProductionPlan.ActiveProcesses[model.RouteId].Alerts.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => x.ItemId == model.MachineId && x.EndAt == null);
                if (alert != null)
                {
                    alert.EndAt = now;
                    alert.StatusId = (int)Constans.AlertStatus.Edited;
                }

            }

            return await UpdateActiveProductionPlanMachine(model.RouteId, model.MachineId, Constans.MACHINE_STATUS.Running, activeProductionPlan);
        }

        private async Task<ActiveProductionPlanModel> UpdateActiveProductionPlanMachine(int routeId, int machineId, int status, ActiveProductionPlanModel activeProductionPlan)
        {
            var dbmachine = _machineRepository.Where(x => x.Id == machineId).FirstOrDefault();
            //when machine is auto
            if (dbmachine.StatusTag == null || dbmachine.StatusTag.Trim() == "")
            {
                dbmachine.StatusId = status;
                _machineRepository.Edit(dbmachine);
            }

            var machine = activeProductionPlan.ActiveProcesses[routeId].Route.MachineList[machineId];
            machine.StatusId = status;
            await _machineService.SetCached(machineId, machine);
            await SetCached(activeProductionPlan);
            await _unitOfWork.CommitAsync();
            return activeProductionPlan;
        }

        public async Task<RecordManufacturingLossModel> GetByGuid(Guid guid)
        {
            var dbModel = await _recordManufacturingLossRepository.GetByGuid(guid);
            var output = MapperHelper.AsModel(dbModel, new RecordManufacturingLossModel());
            output.WasteList = await _recordProductionPlanWasteService.ListByLoss(output.Id);
            output.LossLevelId = output.LossLevel3Id;
            return output;
        }

        public async Task<ActiveProductionPlanModel> Update(RecordManufacturingLossModel model)
        {
            var masterData = await _masterDataService.GetData();
            var dbModel = await _recordManufacturingLossRepository.FirstOrDefaultAsync(x => x.Guid == model.Guid);
            var now = DateTime.Now;
            var activeProductionPlan = await GetCached(model.ProductionPlanId);
            var alert = activeProductionPlan.ActiveProcesses[model.RouteId].Alerts.FirstOrDefault(x => x.Id == Guid.Parse(model.Guid));
            //handle case alert is removed from redis
            if (alert != null)
            {
                alert.LossLevel3Id = model.LossLevelId;
                alert.StatusId = (int)Constans.AlertStatus.Edited;
            }
            dbModel.LossLevel3Id = model.LossLevelId;
            dbModel.MachineId = model.MachineId;
            dbModel.Remark = model.Remark;
            if (model.ComponentId > 0)
            {
                dbModel.ComponentId = model.ComponentId;
            }
            _recordManufacturingLossRepository.Edit(dbModel);

            dbModel = await HandleWaste(dbModel, model, now);

            await _unitOfWork.CommitAsync();
            await SetCached(activeProductionPlan);
            return activeProductionPlan;
        }

        private async Task<RecordManufacturingLoss> HandleWaste(RecordManufacturingLoss dbModel, RecordManufacturingLossModel model, DateTime now)
        {
            await _recordProductionPlanWasteRepository.DeleteByLoss(dbModel.Id);
            var productId = model.WasteList[0].ProductId;
            var productMats = _productmaterialRepository.Where(x => x.ProductId == productId).ToDictionary(t => t.MaterialId, t => t.IngredientPerUnit);
            var mats = _materialRepository.Where(x => productMats.Keys.Contains(x.Id)).ToDictionary(t => t.Id, t => t.BhtperUnit);

            foreach (var item in model.WasteList)
            {
                var waste = MapperHelper.AsModel(item, new RecordProductionPlanWaste());
                foreach (var material in item.Materials)
                {
                    var mat = MapperHelper.AsModel(material, new RecordProductionPlanWasteMaterials());
                    if (item.AmountUnit > 0 && item.IngredientsMaterials.Contains(mat.MaterialId)) mat.Amount += productMats[mat.MaterialId] * item.AmountUnit;
                    mat.Cost = (mat.Amount * mats[mat.MaterialId]);
                    if (mat.Amount > 0) waste.RecordProductionPlanWasteMaterials.Add(mat);
                }

                dbModel.RecordProductionPlanWaste.Add(waste);
                waste.CreatedAt = now;
                waste.CreatedBy = CurrentUser.UserId;
                waste.ProductionPlanId = model.ProductionPlanId;
                waste.CauseMachineId = model.MachineId;

            }
            return dbModel;

        }

        public async Task<PagingModel<RecordManufacturingLossModel>> List(string planId, int? routeId, string keyword, int page, int howmany)
        {
            return await _recordManufacturingLossRepository.ListAsPaging("sp_ListManufacturingLoss", new Dictionary<string, object>()
                {
                    {"@plan_id", planId},
                    {"@route_id", routeId},
                    {"@keyword", keyword},
                    {"@howmany", howmany},
                    {"@page", page}
                }, page, howmany);
        }
    }
}
