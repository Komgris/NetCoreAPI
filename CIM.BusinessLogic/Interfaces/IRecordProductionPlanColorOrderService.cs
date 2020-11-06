using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordProductionPlanColorOrderService : IBaseService
    {
        Task<RecordProductionPlanColorOrderModel> Compare(RecordProductionPlanColorOrderModel model);
        Task<RecordProductionPlanColorOrderModel> Update(RecordProductionPlanColorOrderModel model);
        Task<RecordProductionPlanColorOrderModel> Create(RecordProductionPlanColorOrderModel model);
        Task<List<RecordProductionPlanColorOrderModel>> List(string planId);
    }
}
