using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class RecordProductionPlanCheckListService : BaseService, IRecordProductionPlanCheckListService
    {
        private IResponseCacheService _responseCacheService;
        private IRecordProductionPlanCheckListRepository _recordProductionPlanCheckListRepository;
        private IRecordProductionPlanCheckListDetailRepository _recordProductionPlanCheckListDetailRepository;
        private IDirectSqlRepository _directSqlRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanCheckListService(
        IResponseCacheService responseCacheService,
        IRecordProductionPlanCheckListRepository recordProductionPlanCheckListRepository,
        IRecordProductionPlanCheckListDetailRepository recordProductionPlanCheckListDetailRepository,
        IDirectSqlRepository directSqlRepository,
        IUnitOfWorkCIM unitOfWork
        )
        {
            _responseCacheService = responseCacheService;
            _directSqlRepository = directSqlRepository;
            _recordProductionPlanCheckListRepository = recordProductionPlanCheckListRepository;
            _recordProductionPlanCheckListDetailRepository = recordProductionPlanCheckListDetailRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RecordProductionPlanCheckListModel> Create(RecordProductionPlanCheckListModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new RecordProductionPlanCheckList());
            dbModel.CreatedAt = DateTime.Now;
            dbModel.CreatedBy = CurrentUser.UserId;
            foreach (var datails in model.checkListdetail)
            {
                var checklist = MapperHelper.AsModel(datails, new RecordProductionPlanCheckListDetail());
                dbModel.RecordProductionPlanCheckListDetail.Add(checklist);
            }

            if (dbModel.RecordProductionPlanCheckListDetail.Count > 0)
            {
                    _recordProductionPlanCheckListRepository.Add(dbModel);
            }
            await _unitOfWork.CommitAsync();
            return model;
        }

        public async Task<RecordProductionPlanCheckListModel> Update(RecordProductionPlanCheckListModel model,int recordId)
        {
            var checklistTyp = model.checkListdetail[0].CheckListTypeId;
            var recordChecklist = await _recordProductionPlanCheckListRepository.Get(recordId);
            var recordChecklistType = recordChecklist.RecordProductionPlanCheckListDetail.Where(x => x.CheckListTypeId == checklistTyp);
            foreach (var item in recordChecklistType)
            {
                _recordProductionPlanCheckListDetailRepository.Delete(item);
            }
            recordChecklist.RecordProductionPlanCheckListDetail.ToList().RemoveAll(x => x.CheckListTypeId == checklistTyp);

            foreach (var datails in model.checkListdetail)
            {
                var checklist = MapperHelper.AsModel(datails, new RecordProductionPlanCheckListDetail());
                recordChecklist.RecordProductionPlanCheckListDetail.Add(checklist);
            }

            if (recordChecklist.RecordProductionPlanCheckListDetail.Count > 0)
            {
                recordChecklist.UpdatedAt = DateTime.Now;
                recordChecklist.UpdatedBy = CurrentUser.UserId;
                _recordProductionPlanCheckListRepository.Edit(recordChecklist);
            }
            await _unitOfWork.CommitAsync();
                return model;

        }

        public async Task<RecordProductionPlanCheckListModel> Compare(RecordProductionPlanCheckListModel model)
        {
            var recordChecklist = (await _recordProductionPlanCheckListRepository.FirstOrDefaultAsync(x => x.ProductionPlanId == model.ProductionPlanId));
            if(recordChecklist == null)
            {
                return await Create(model);
            }
            else
            {
                return await Update(model, recordChecklist.Id);
            }
        }

        public async Task<List<RecordProductionPlanCheckListModel>> List(string planId, int? checklistTypeId)
        {
            var output = await _recordProductionPlanCheckListRepository.List("sp_ListRecordCheckList", new Dictionary<string, object>()
                {
                    {"@planId", planId},
                    {"@checklist_Id", checklistTypeId}
                });
            return output;
        }

        public async Task<bool> Validate(string planId)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
            };
            var isCheck = _directSqlRepository.ExecuteFunction<bool>("dbo.fn_validation_plan_checklist", paramsList);
            return isCheck;
        }
    }
}
