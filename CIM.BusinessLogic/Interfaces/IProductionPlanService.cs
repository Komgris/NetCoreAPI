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
        string GetProductionPlanKey(string id);
        List<ProductionPlanModel> Get();
        Task<PagingModel<ProductionPlanModel>> Paging(int page, int howmany);
        Task<List<ProductionPlanModel>> CheckDuplicate(List<ProductionPlanModel> import);
        Task Delete(string id);
        Task Update(ProductionPlanModel model);
        Task<ProductionPlanModel> Create(ProductionPlanModel model);
        Task<List<ProductionPlanModel>> Compare(List<ProductionPlanModel> import);
        List<ProductionPlanModel> ReadImport(string path);
        List<ProductionPlanModel> ConvertImportToList(ExcelWorksheet oSheet);
        Task<ActiveProductionPlanModel> Start(ProductionPlanModel model);
        Task Finish(ProductionPlanModel model);
        Task<ProductionPlanModel> Get(string id);
        Task Pause(string id, int routeId);
        Task Resume(string id, int routeId);
        Task<PagingModel<ProductionPlanListModel>> List(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive, string statusIds);
        Task<ProductionPlanModel> Load(string id);
        Task<ActiveProductionPlanModel> TakeAction(string id);
        Task<ActiveProductionPlanModel> UpdateByMachine(int id, int statusId);
    }
}
