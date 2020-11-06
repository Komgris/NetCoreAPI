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

        public async Task<RecordProductionPlanInformationModel> Compare(RecordProductionPlanInformationModel model)
        {
            var recordChecklist = (await _recordProductionPlanInformationRepository.FirstOrDefaultAsync(x => x.ProductionPlanId == model.ProductionPlanId));
            if (recordChecklist == null)
            {
                return await Create(model);
            }
            else
            {
                return await Update(model);
            }
        }

        public async Task<RecordProductionPlanInformationModel> Create(RecordProductionPlanInformationModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new RecordProductionPlanInformation());
            dbModel.CreatedAt = DateTime.Now;
            dbModel.CreatedBy = CurrentUser.UserId;
            foreach (var datails in model.Informationdetail)
            {
                var colorList = MapperHelper.AsModel(datails, new RecordProductionPlanInformationDetail());
                dbModel.RecordProductionPlanInformationDetail.Add(colorList);
            }

            if (dbModel.RecordProductionPlanInformationDetail.Count > 0)
            {
                _recordProductionPlanInformationRepository.Add(dbModel);
            }
            await _unitOfWork.CommitAsync();
            return model;
        }

        public async Task<RecordProductionPlanInformationModel> Update(RecordProductionPlanInformationModel model)
        {
            var dbModel = await _recordProductionPlanInformationRepository.Get(model.Id);
            foreach (var item in dbModel.RecordProductionPlanInformationDetail)
            {
                _recordProductionPlanInformationDetailRepository.Delete(item);
            }
            dbModel.RecordProductionPlanInformationDetail.Clear();
            foreach (var datails in model.Informationdetail)
            {
                var infoList = MapperHelper.AsModel(datails, new RecordProductionPlanInformationDetail());
                dbModel.RecordProductionPlanInformationDetail.Add(infoList);
            }

            if (dbModel.RecordProductionPlanInformationDetail.Count > 0)
            {
                dbModel.UpdatedAt = DateTime.Now;
                dbModel.UpdatedBy = CurrentUser.UserId;
                _recordProductionPlanInformationRepository.Edit(dbModel);
            }
            await _unitOfWork.CommitAsync();
            return model;
        }
    }
}
