using CIM.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IProductionPlanService : IBaseService
    {
        List<ProductionPlanModel> Get();
        Task<PagingModel<ProductionPlanModel>> Paging(int page, int howmany);
        Task<List<ProductionPlanModel>> Create(List<ProductionPlanModel> import);
        Task Delete(string id);
        void Update(List<ProductionPlanModel> list);
        List<ProductionPlanModel> Compare(List<ProductionPlanModel> import, List<ProductionPlanModel> dbPlan);
        List<ProductionPlanModel> ReadImport(string path);
        List<ProductionPlanModel> ConvertImportToList(ExcelWorksheet oSheet);
        Task Start(ProductionPlanModel model);
        Task<ProductionPlanModel> Get(string id);
        Task<ActiveProcessModel> UpdateByComponent(int id, int statusId);
        Task Stop(string id);
        Task<PagingModel<ProductionPlanListModel>> List(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive);
        Task<ProductionPlanModel> Load(string id);
        Task<ActiveProcessModel> TakeAction(int id);
    }
}
