using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RecordManufacturingLossService : BaseService, IRecordManufacturingLossService
    {
        private IRecordManufacturingLossRepository _recordManufacturingLossRepository;
        private IMasterDataService _masterDataService;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordManufacturingLossService(
            IRecordManufacturingLossRepository recordManufacturingLossRepository,
            IMasterDataService masterDataService,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _recordManufacturingLossRepository = recordManufacturingLossRepository;
            _masterDataService = masterDataService;
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
            await _unitOfWork.CommitAsync();
        }
    }
}
