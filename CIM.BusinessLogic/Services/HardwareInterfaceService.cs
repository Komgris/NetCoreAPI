using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;

namespace CIM.BusinessLogic.Services
{
    public class HardwareInterfaceService : BaseService, IHardwareInterfaceService
    {
        private readonly IProductionOutputRepository _productionoutputrepository;
        private IUnitOfWorkCIM _unitOfWork;
        public HardwareInterfaceService(
            IProductionOutputRepository productionoutputrepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _productionoutputrepository = productionoutputrepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool>  OutputUpdate(RecordOutputModel model)
        {
            var dbModel = new RecordProductionOutput
            {
                ProductionPlanId = model.OutputId.ToString(),
                Count = model.Count,
                CreatedBy = CurrentUser.UserId,
                CreatedAt = DateTime.Now

            };
            _productionoutputrepository.Add(dbModel);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
