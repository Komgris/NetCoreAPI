using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
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
        private IUnitOfWorkCIM _unitOfWork;

        public RecordProductionPlanCheckListService(
        IResponseCacheService responseCacheService,
        IRecordProductionPlanCheckListRepository recordProductionPlanCheckListRepository,
        IRecordProductionPlanCheckListDetailRepository recordProductionPlanCheckListDetailRepository,
        IUnitOfWorkCIM unitOfWork
        )
        {
            _responseCacheService = responseCacheService;
            _recordProductionPlanCheckListRepository = recordProductionPlanCheckListRepository;
            _recordProductionPlanCheckListDetailRepository = recordProductionPlanCheckListDetailRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RecordProductionPlanCheckListModel> Create(RecordProductionPlanCheckListModel model)
        {
            var dbModel = MapperHelper.AsModel(model, new RecordProductionPlanCheckList());
            dbModel.CreatedAt = DateTime.Now;
            dbModel.CreatedBy = CurrentUser.UserId;
            //var recordChecklist = await _recordProductionPlanCheckListRepository.Get(dbModel.Id);
            //var detailList = recordChecklist.RecordProductionPlanCheckListDetail.ToList();

            //var detail = await _recordProductionPlanCheckListDetailRepository.Get(model.checkListdetail[0].Id);
            //var recordChecklist = (await _recordProductionPlanCheckListRepository.FirstOrDefaultAsync(x => x.ProductionPlanId == dbModel.ProductionPlanId));
            //var recordChecklistDetail = (await _recordProductionPlanCheckListDetailRepository.FirstOrDefaultAsync(x => x.RecordCheckListId == recordChecklist.Id && ));
            foreach (var datails in model.checkListdetail)
            {
                var checklist = MapperHelper.AsModel(datails, new RecordProductionPlanCheckListDetail());
                //var existId = detailList.Where(x => x.CheckListId == datails.CheckListId);
                //foreach(var duplicate in existId)
                //{
                //    _recordProductionPlanCheckListDetailRepository.Delete(duplicate);
                //}
                dbModel.RecordProductionPlanCheckListDetail.Add(checklist);
            }

            if (dbModel.RecordProductionPlanCheckListDetail.Count > 0)
            {
                    _recordProductionPlanCheckListRepository.Add(dbModel);
            }
            await _unitOfWork.CommitAsync();
            return model;
        }

        public async Task<RecordProductionPlanCheckListModel> Update(RecordProductionPlanCheckListModel model)
        {
            var checklistTyp = model.checkListdetail[0].CheckListTypeId;
            if(model.Id == 0)
            {
                return await Create(model);
            }
            else
            {
                var recordChecklist = await _recordProductionPlanCheckListRepository.Get(model.Id);
                var recordChecklistType = recordChecklist.RecordProductionPlanCheckListDetail.ToList().Where(x => x.CheckListTypeId == checklistTyp);
                foreach (var item in recordChecklistType)
                {
                    _recordProductionPlanCheckListDetailRepository.Delete(item);
                }
                recordChecklist.RecordProductionPlanCheckListDetail.Clear();
                //var detailList = recordChecklist.RecordProductionPlanCheckListDetail.ToList();

                //var detail = await _recordProductionPlanCheckListDetailRepository.Get(model.checkListdetail[0].Id);
                //var recordChecklist = (await _recordProductionPlanCheckListRepository.FirstOrDefaultAsync(x => x.ProductionPlanId == dbModel.ProductionPlanId));
                //var recordChecklistDetail = (await _recordProductionPlanCheckListDetailRepository.FirstOrDefaultAsync(x => x.RecordCheckListId == recordChecklist.Id && ));
                foreach (var datails in model.checkListdetail)
                {
                    var checklist = MapperHelper.AsModel(datails, new RecordProductionPlanCheckListDetail());
                    //var existId = detailList.Where(x => x.CheckListId == datails.CheckListId);
                    //foreach(var duplicate in existId)
                    //{
                    //    _recordProductionPlanCheckListDetailRepository.Delete(duplicate);
                    //}
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
                return await Update(model);
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
    }
}
