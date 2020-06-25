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
    public class MachineOperatorService : BaseService, IMachineOperatorService
    {
        private IMachineOperatorRepository _machineOperatorRepository;
        private IUnitOfWorkCIM _unitOfWorkCIM;

        public MachineOperatorService(
            IMachineOperatorRepository machineOperatorRepository,
            IUnitOfWorkCIM unitOfWorkCIM

            )
        {
            _machineOperatorRepository = machineOperatorRepository;
            _unitOfWorkCIM = unitOfWorkCIM;
        }

        public async Task Create(MachineOperatorModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new MachineOperators());
            dbModel.LastUpdatedAt = DateTime.Now;
            dbModel.LastUpdatedBy = CurrentUser.UserId;
            _machineOperatorRepository.Add(dbModel);
            await _unitOfWorkCIM.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var dbModel = await _machineOperatorRepository.FirstOrDefaultAsync(x => x.Id == id);
            _machineOperatorRepository.Delete(dbModel);
            await _unitOfWorkCIM.CommitAsync();
        }

        public async Task Update(MachineOperatorModel model)
        {
            var dbModel = await _machineOperatorRepository.FirstOrDefaultAsync(x => x.MachineId == model.MachineId && x.PlanId == model.PlanId);
            dbModel.OperatorCount = model.OperatorCount;
            dbModel.LastUpdatedAt = DateTime.Now;
            dbModel.LastUpdatedBy = CurrentUser.UserId;
            _machineOperatorRepository.Edit(dbModel);
            await _unitOfWorkCIM.CommitAsync();
        }
    }
}
