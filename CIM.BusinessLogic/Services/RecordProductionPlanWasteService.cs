using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
