using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RecordProductionPlanWasteService : BaseService, IRecordProductionPlanWasteService
    {
        private IRecordProductionPlanWasteMaterialRepository _recordProductionPlanWasteMaterialRepository;
        private IRecordProductionPlanWasteRepository _recordProductionPlanWasteRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanWasteService(
            IRecordProductionPlanWasteMaterialRepository recordProductionPlanWasteMaterialRepository,
            IRecordProductionPlanWasteRepository recordProductionPlanWasteRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _recordProductionPlanWasteMaterialRepository = recordProductionPlanWasteMaterialRepository;
            _recordProductionPlanWasteRepository = recordProductionPlanWasteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RecordProductionPlanWasteModel> Create(RecordProductionPlanWasteModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new RecordProductionPlanWaste());
            dbModel.CreatedAt = DateTime.Now;
            dbModel.CreatedBy = CurrentUser.UserId;

            foreach (var material in model.Materials)
            {
                var mat = MapperHelper.AsModel(material, new RecordProductionPlanWasteMaterials());
                dbModel.RecordProductionPlanWasteMaterials.Add(mat);
            }

            _recordProductionPlanWasteRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
            return model;
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

        public async Task Update(RecordProductionPlanWasteModel model)
        {
            var dbModel = await _recordProductionPlanWasteRepository.Get(model.Id);
            foreach (var item in dbModel.RecordProductionPlanWasteMaterials)
            {
                _recordProductionPlanWasteMaterialRepository.Delete(item);
            }

            foreach (var material in model.Materials)
            {
                var mat = MapperHelper.AsModel(material, new RecordProductionPlanWasteMaterials());
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
    }
}
