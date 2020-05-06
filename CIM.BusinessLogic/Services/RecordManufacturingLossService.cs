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
    public class RecordManufacturingLossService : BaseService, IRecordManufacturingLossService
    {
        private IRecordManufacturingLossRepository _recordManufacturingLossRepository;
        private IMasterDataService _masterDataService;
        private IRecordProductionPlanWasteService _recordProductionPlanWasteService;
        private IRecordProductionPlanWasteRepository _recordProductionPlanWasteRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordManufacturingLossService(
            IRecordManufacturingLossRepository recordManufacturingLossRepository,
            IMasterDataService masterDataService,
            IRecordProductionPlanWasteService recordProductionPlanWasteService,
            IRecordProductionPlanWasteRepository recordProductionPlanWasteRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _recordManufacturingLossRepository = recordManufacturingLossRepository;
            _masterDataService = masterDataService;
            _recordProductionPlanWasteService = recordProductionPlanWasteService;
            _recordProductionPlanWasteRepository = recordProductionPlanWasteRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task Create(RecordManufacturingLossModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new RecordManufacturingLoss());
            _recordManufacturingLossRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
        }

        public async Task<RecordManufacturingLossModel> GetByGuid(Guid guid)
        {
            var dbModel = await _recordManufacturingLossRepository.GetByGuid(guid);
            var output = MapperHelper.AsModel(dbModel, new RecordManufacturingLossModel());
            output.WasteList = await _recordProductionPlanWasteService.ListByLoss(output.Id);
            return output;
        }

        public async Task Update(RecordManufacturingLossModel model)
        {
            var masterData = await _masterDataService.GetData();
            var dbModel = await _recordManufacturingLossRepository.FirstOrDefaultAsync(x => x.Guid == model.Guid);
            var now = DateTime.Now;
            dbModel.LossLevel3Id = model.LossLevelId;
            dbModel.MachineId = model.MachineId;
            if (model.ComponentId > 0)
            {
                dbModel.ComponentTypeId = masterData.Components[model.ComponentId.Value].TypeId;
            }
            dbModel.LossLevel3Id = model.LossLevelId;
            _recordManufacturingLossRepository.Edit(dbModel);
            await _recordProductionPlanWasteRepository.DeleteByLoss(dbModel.Id);
            foreach (var item in model.WasteList)
            {

                var waste = MapperHelper.AsModel(item, new RecordProductionPlanWaste());
                foreach (var material in item.Materials)
                {
                    var mat = MapperHelper.AsModel(material, new RecordProductionPlanWasteMaterials());
                    waste.RecordProductionPlanWasteMaterials.Add(mat);
                }

                waste.CreatedAt = now;
                waste.CreatedBy = CurrentUser.UserId;
                waste.ProductionPlanId = model.ProductionPlanId;
                waste.CauseMachineId = model.MachineId;
                _recordProductionPlanWasteRepository.Add(waste);

            }
            
            await _unitOfWork.CommitAsync();
        }

    }
}
