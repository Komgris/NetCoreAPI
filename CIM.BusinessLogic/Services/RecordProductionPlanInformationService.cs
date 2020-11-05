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

        //public async Task<RecordProductionPlanInformationModel> Compare(RecordProductionPlanInformationModel model)
        //{
        //    var recordChecklist = (await _recordProductionPlanInformationRepository.FirstOrDefaultAsync(x => x.ProductionPlanId == model.ProductionPlanId));
        //    if (recordChecklist == null)
        //    {
        //        return await Create(model);
        //    }
        //    else
        //    {
        //        return await Update(model);
        //    }
        //}

        //public async Task<RecordProductionPlanInformationModel> Create(RecordProductionPlanInformationModel model)
        //{
        //    var dbModel = MapperHelper.AsModel(model, new RecordProductionPlanInformation());
        //    dbModel.CreatedAt = DateTime.Now;
        //    dbModel.CreatedBy = CurrentUser.UserId;
        //    foreach (var datails in model.Colordetail)
        //    {
        //        var colorList = MapperHelper.AsModel(datails, new RecordProductionPlanColorOrderDetail());
        //        colorList.Sequence = order;
        //        dbModel.RecordProductionPlanColorOrderDetail.Add(colorList);
        //    }

        //    if (dbModel.RecordProductionPlanColorOrderDetail.Count > 0)
        //    {
        //        _recordProductionPlanColorOrderRepository.Add(dbModel);
        //    }
        //    await _unitOfWork.CommitAsync();
        //    return model;
        //}

        //public async Task<>
    }
}
