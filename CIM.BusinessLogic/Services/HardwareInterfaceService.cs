using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CIM.BusinessLogic.Services
{
    public class HardwareInterfaceService : BaseService, IHardwareInterfaceService
    {
        private readonly IMachineComponentLossRepository _machinecomponentlossRepository;
        private readonly IMachineComponentStatusRepository _machinecomponentstatusRepository;
        private readonly IProductionOutputRepository _productionoutputRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public HardwareInterfaceService(
            IUnitOfWorkCIM unitOfWork,
            IMachineComponentLossRepository machinecomponentlossRepository,
            IMachineComponentStatusRepository machinecomponentstatusRepository,
            IProductionOutputRepository productionoutputRepository
            )
        {
            _machinecomponentlossRepository = machinecomponentlossRepository;
            _machinecomponentstatusRepository = machinecomponentstatusRepository;
            _productionoutputRepository = productionoutputRepository;

            _unitOfWork = unitOfWork;
        }


        public async Task<bool> UpdateOutput(RecordOutputModel model)
        {
            var dbModel = new RecordProductionOutput()
            {
                Count = model.Count,
                ProductionPlanId = "TestAPI",
                CreatedBy = CurrentUser.UserId,
            };
            _productionoutputRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> UpdateStatus(MachineStatusModel model)
        {
            //to do
            //convert to mcStatus and use enum to map
            //var mcstatus = model.IsRunning ?? 2:3;

            //toDo continue
            var dbModel = new RecordMachineComponentStatus()
            {
                MachineComponentId = model.Id,
                MachineStatusId = Convert.ToInt32(model.IsRunning),
                CreatedBy = CurrentUser.UserId
            };

            _machinecomponentstatusRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> UpdateLoss(MachineStatusModel model)
        {
            return true;
        }
    }
}
