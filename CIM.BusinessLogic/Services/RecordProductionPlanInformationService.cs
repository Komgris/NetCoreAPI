using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Services
{
    public class RecordProductionPlanInformationService : BaseService, IRecordProductionPlanInformationService
    {
        private IRecordProductionPlanInformationRepository _recordProductionPlanInformationRepository;
        private IRecordProductionPlanInformationDetailRepository _recordProductionPlanInformationDetailRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanInformationService(
            IRecordProductionPlanInformationRepository recordProductionPlanInformationRepository,
            IRecordProductionPlanInformationDetailRepository recordProductionPlanInformationDetailRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _recordProductionPlanInformationDetailRepository = recordProductionPlanInformationDetailRepository;
            _recordProductionPlanInformationRepository = recordProductionPlanInformationRepository;
            _unitOfWork = unitOfWork;
        }

        //public async Task<>
    }
}
