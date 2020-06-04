using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CIM.BusinessLogic.Services
{
    public class RecordProductionPlanOutputService : BaseService, IRecordProductionPlanOutputService
    {
        private IResponseCacheService _responseCacheService;
        private IRecordProductionPlanOutputRepository _recordProductionPlanOutputRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanOutputService(
            IResponseCacheService responseCacheService,
            IRecordProductionPlanOutputRepository recordProductionPlanOutputRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _responseCacheService = responseCacheService;
            _recordProductionPlanOutputRepository = recordProductionPlanOutputRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProcessReponseModel<bool>> UpdateMachineProduceCounter(List<MachineProduceCounterModel> listData, int hour)
        {

            return null;
        }
    }
}
