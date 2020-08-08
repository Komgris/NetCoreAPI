﻿using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RecordProductionPlanWasteService : BaseService, IRecordProductionPlanWasteService
    {
        private IRecordProductionPlanWasteMaterialRepository _recordProductionPlanWasteMaterialRepository;
        private IRecordProductionPlanWasteRepository _recordProductionPlanWasteRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private IDirectSqlRepository _directSqlRepository;
        private IMaterialRepository _materialRepository;
        private IProductMaterialRepository _productmaterialRepository;

        public RecordProductionPlanWasteService(
            IRecordProductionPlanWasteMaterialRepository recordProductionPlanWasteMaterialRepository,
            IRecordProductionPlanWasteRepository recordProductionPlanWasteRepository,
            IUnitOfWorkCIM unitOfWork,
            IDirectSqlRepository directSqlRepository,
            IMaterialRepository materialRepository,
            IProductMaterialRepository productmaterialRepository
            )
        {
            _recordProductionPlanWasteMaterialRepository = recordProductionPlanWasteMaterialRepository;
            _recordProductionPlanWasteRepository = recordProductionPlanWasteRepository;
            _unitOfWork = unitOfWork;
            _directSqlRepository = directSqlRepository;
            _materialRepository = materialRepository;
            _productmaterialRepository = productmaterialRepository;
        }

        public async Task<RecordProductionPlanWasteModel> Create(RecordProductionPlanWasteModel model)
        {
            var rediskey = $"{Constans.RedisKey.ACTIVE_PRODUCTION_PLAN}:{model.ProductionPlanId}";
            var dbModel = MapperHelper.AsModel(model, new RecordProductionPlanWaste());
            dbModel.CreatedAt = DateTime.Now;
            dbModel.CreatedBy = CurrentUser.UserId;

            var productMats = _productmaterialRepository.Where(x => x.ProductId == model.ProductId).ToDictionary(t => t.MaterialId, t => t.IngredientPerUnit); 
            var mats = _materialRepository.Where(x => model.IngredientsMaterials.Contains(x.Id)).ToDictionary(t => t.Id, t => t.BhtperUnit);

            foreach (var material in model.Materials)
            {
                var mat = MapperHelper.AsModel(material, new RecordProductionPlanWasteMaterials());
                if(model.AmountUnit >0) mat.Amount += productMats[mat.Id] * model.AmountUnit;
                mat.Cost = (mat.Amount * mats[mat.Id].Value);
                dbModel.RecordProductionPlanWasteMaterials.Add(mat);
            }

            _recordProductionPlanWasteRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
            return model;
        }
        public async Task Update(RecordProductionPlanWasteModel model)
        {
            var dbModel = await _recordProductionPlanWasteRepository.Get(model.Id);
            foreach (var item in dbModel.RecordProductionPlanWasteMaterials)
            {
                _recordProductionPlanWasteMaterialRepository.Delete(item);
            }
            
            var mats = _materialRepository.Where(x => model.IngredientsMaterials.Contains(x.Id)).ToDictionary(t => t.Id, t => t.BhtperUnit);
            foreach (var material in model.Materials)
            {
                var mat = MapperHelper.AsModel(material, new RecordProductionPlanWasteMaterials()); 
                mat.Cost = (mat.Amount * mats[mat.Id]);
                dbModel.RecordProductionPlanWasteMaterials.Add(mat);
            }
            dbModel.CauseMachineId = model.CauseMachineId;
            dbModel.Reason = model.Reason;
            dbModel.RouteId = model.RouteId;
            dbModel.UpdatedAt = DateTime.Now;
            dbModel.UpdatedBy = CurrentUser.UserId;
            dbModel.WasteLevel2Id = model.WasteLevel2Id;
            _recordProductionPlanWasteRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();

        }

        public async Task NonePrimeCreate(List<RecordProductionPlanWasteNonePrimeModel> models)
        {
            //store proc.
            var paramsList = new Dictionary<string, object>();
            foreach (var item in models)
            {
                 paramsList = new Dictionary<string, object>()
                {
                    {"@plan_id", item.ProductionPlanId},
                    {"@route_id", item.RouteId},        
                    {"@noneprime_id", item.NonePrimeId},
                    {"@amount", item.Amount},
                    {"@user", CurrentUserId},
                };
                await Task.Run(() => _directSqlRepository.ExecuteSPNonQuery("sp_Record_Waste_NonePrime", paramsList));
            }
        }

        public async Task<DataTable> RecordNonePrimeList(string planId, int routeId)
        {
            //store proc.
            var paramsList = new Dictionary<string, object>()
                {
                    {"@plan_id", planId},
                    {"@route_id", routeId}
                };
            return await Task.Run(() => _directSqlRepository.ExecuteSPWithQuery("sp_ListWasteNonePrime", paramsList));

        }

        public async Task Delete(int id)
        {
            var dbModel = await _recordProductionPlanWasteRepository.FirstOrDefaultAsync(x => x.Id == id);
            dbModel.IsDelete = true;
            _recordProductionPlanWasteRepository.Edit(dbModel);
            await _unitOfWork.CommitAsync();
        }

        public async Task<RecordProductionPlanWasteModel> Get(int id)
        {
            var dbModel = await _recordProductionPlanWasteRepository.Get(id);
            var output = MapperHelper.AsModel(dbModel, new RecordProductionPlanWasteModel(), new[] { "WasteLevel2" });
            output.Materials = dbModel.RecordProductionPlanWasteMaterials.Select(x => MapperHelper.AsModel(x, new RecordProductionPlanWasteMaterialModel())).ToList();
            return output;
        }

        public async Task<PagingModel<RecordProductionPlanWasteModel>> List(string planId, int? routeId, string keyword, int page, int howmany)
        {
            return await _recordProductionPlanWasteRepository.ListAsPaging("[dbo].[sp_ListWaste]", new Dictionary<string, object>()
                {
                    {"@plan_id", planId},
                    {"@route_id", routeId},
                    {"@keyword", keyword},
                    {"@howmany", howmany},
                    { "@page", page}
                }, page, howmany);
        }

        public async Task<List<RecordProductionPlanWasteModel>> ListByLoss(int lossId)
        {
            var wasteMaterials = await _recordProductionPlanWasteMaterialRepository.ListByLoss(lossId);
            var output = await _recordProductionPlanWasteRepository.ListByLoss(lossId);
            foreach (var item in output)
            {
                item.Materials = wasteMaterials.Where(x => x.WasteId == item.Id).ToList();
            }
            return output;
        }

    }
}
