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

        public async Task Update(RecordManufacturingLossModel model)
        {
            var masterData = await _masterDataService.GetData();
            var dbModel = await _recordManufacturingLossRepository.FirstOrDefaultAsync(x => x.Guid == model.Guid);
            dbModel.LossLevel3Id = model.LossLevelId;
            dbModel.MachineId = model.MachineId;
            if (model.ComponentId > 0)
            {
                dbModel.ComponentTypeId = masterData.Components[model.ComponentId.Value].TypeId;
            }
            dbModel.LossLevel3Id = model.LossLevelId;
            _recordManufacturingLossRepository.Edit(dbModel);
            var updateWasteList = _recordProductionPlanWasteService.ToDictiony(model.WasteList.Where(x => x.Id != 0));
            var createWasteList = model.WasteList.Where(x => x.Id == 0);
            var lossWaste = await _recordProductionPlanWasteRepository.ListByLoss(model.Id);
            var now = DateTime.Now;
            foreach (var waste in lossWaste)
            {
                var updateModel = updateWasteList[waste.Id];
                waste.CauseMachineId = updateModel.CauseMachineId;
                waste.IsDelete = updateModel.IsDelete;
                waste.Reason = updateModel.Reason;
                waste.RouteId = updateModel.RouteId;
                waste.UpdatedBy = CurrentUser.UserId;
                waste.UpdatedAt = now;
                _recordProductionPlanWasteRepository.Edit(waste);
            }

            foreach (var item in createWasteList)
            {
                var createModel = MapperHelper.AsModel(item, new RecordProductionPlanWaste());
                createModel.CreatedAt = now;
                createModel.CreatedBy = CurrentUser.UserId;
                _recordProductionPlanWasteRepository.Add(createModel);
            }
            await _unitOfWork.CommitAsync();
        }

    }
}
