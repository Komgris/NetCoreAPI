using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
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


        public Task<bool> UpdateOutput(RecordOutputModel model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStatus(MachineStatusModel model)
        {
            throw new NotImplementedException();
        }
    }
}
