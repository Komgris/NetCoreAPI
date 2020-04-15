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
        Task<List<ProductionPlanModel>> CheckDuplicate(List<ProductionPlanModel> import);
        Task Delete(string id);
        Task Update(ProductionPlanModel model);
        Task<ProductionPlanModel> Create(ProductionPlanModel model);
        Task<List<ProductionPlanModel>> Compare(List<ProductionPlanModel> import);
        List<ProductionPlanModel> ReadImport(string path);
        List<ProductionPlanModel> ConvertImportToList(ExcelWorksheet oSheet);
        Task<ActiveProcessModel> Start(ProductionPlanModel model);
        Task<ProductionPlanModel> Get(string planId);
        Task Stop(string id, int[] routeId);
        Task<PagingModel<ProductionPlanListModel>> List(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive);
        Task<ProductionPlanModel> Load(string id);
        Task<ActiveProcessModel> TakeAction(int id);

    }
}
