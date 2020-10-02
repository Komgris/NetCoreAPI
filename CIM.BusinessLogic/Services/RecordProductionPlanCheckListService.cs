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
    public class RecordProductionPlanCheckListService : BaseService, IRecordProductionPlanCheckListService
    {
        private IResponseCacheService _responseCacheService;
        private IRecordProductionPlanCheckListRepository _recordProductionPlanCheckListRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanCheckListService(
        IResponseCacheService responseCacheService,
        IRecordProductionPlanCheckListRepository recordProductionPlanCheckListRepository,
        IUnitOfWorkCIM unitOfWork
        )
        {
            _responseCacheService = responseCacheService;
            _recordProductionPlanCheckListRepository = recordProductionPlanCheckListRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RecordProductionPlanCheckListModel> Create(RecordProductionPlanCheckListModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new RecordProductionPlanCheckList());
            dbModel.CreatedAt = DateTime.Now;
            dbModel.CreatedBy = CurrentUser.UserId;

            foreach (var datails in model.checkListdetail)
            {
                var checklist = MapperHelper.AsModel(datails, new RecordProductionPlanCheckListDetail(), new[] { "Id" });
                //if(model.AmountUnit >0 && model.IngredientsMaterials.Contains(mat.MaterialId)) mat.Amount += productMats[mat.MaterialId] * model.AmountUnit;
                //mat.Cost = (mat.Amount * mats[mat.MaterialId]);
                //if (mat.Amount > 0) 
                dbModel.RecordProductionPlanCheckListDetail.Add(checklist);
            }

            if (dbModel.RecordProductionPlanCheckListDetail.Count > 0)
            {
                _recordProductionPlanCheckListRepository.Add(dbModel);
            }
            await _unitOfWork.CommitAsync();
            return model;
        }

        public async Task<List<RecordProductionPlanCheckListModel>> List(string planId)
        {
            var output = await _recordProductionPlanCheckListRepository.List("sp_ListRecordCheckList", new Dictionary<string, object>()
                {
                    {"@planId", planId}
                });
            return output;
        }
    }
}
