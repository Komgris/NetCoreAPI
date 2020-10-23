using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordProductionPlanCheckListService : IBaseService
    {
        Task<RecordProductionPlanCheckListModel> Create(RecordProductionPlanCheckListModel model);
        Task<List<RecordProductionPlanCheckListModel>> List(string planId, int? checklistTypeId);
        Task<RecordProductionPlanCheckListModel> Compare(RecordProductionPlanCheckListModel model);
        Task<bool> Validate(string planId);
    }
}
