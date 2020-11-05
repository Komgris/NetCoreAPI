using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Services
{
    public class RecordProductionPlanColorOrderService :BaseService, IRecordProductionPlanColorOrderService
    {
        private IRecordProductionPlanColorOrderRepository _recordProductionPlanColorOrderRepository;
        private IRecordProductionPlanColorOrderDetailRepository _recordProductionPlanColorOrderDetailRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanColorOrderService(
            IRecordProductionPlanColorOrderRepository recordProductionPlanColorOrderRepository,
            IRecordProductionPlanColorOrderDetailRepository recordProductionPlanColorOrderDetailRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _recordProductionPlanColorOrderRepository = recordProductionPlanColorOrderRepository;
            _recordProductionPlanColorOrderDetailRepository = recordProductionPlanColorOrderDetailRepository;
            _unitOfWork = unitOfWork;
        }
    }
}
