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
        Task<List<ProductionPlan3MModel>> CheckDuplicate(List<ProductionPlan3MModel> import);
        Task<List<ProductionMasterShowModel>> DeletePlans(List<ProductionMasterShowModel> data);
        Task Delete(string id);
        Task Update(ProductionPlanModel model);
        Task<ProductionPlanModel> Create(ProductionPlanModel model);
        Task<List<ProductionPlanModel>> Compare(List<ProductionPlanModel> import);
        Task<List<ProductionPlanModel>> ReadImport(string path);
        Task<List<ProductionPlanModel>> ConvertImportToList(ExcelWorksheet oSheet);
        Task<List<ProductionPlanListModel>> ListByMonth(int month, int year, string statusIds);
        Task<List<ProductionOutputModel>> ListOutputByMonth(int month, int year);
        Task<PagingModel<ProductionOutputModel>> ListOutputByDate(DateTime date, int page, int howmany);
        Task<PagingModel<ProductionPlanListModel>> ListByDate(DateTime date, int page, int howmany, string statusIds, int? processTypeId);
        Task<ProductionPlanModel> Get(string id);
        Task<PagingModel<ProductionPlan3MModel>> List(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive, string statusIds, int? machineId);
        Task<PagingModel<ProductionOutputModel>> ListOutput(int page, int howmany, string keyword, bool isActive, string statusIds);
        Task<List<ProductionPlan3MModel>> validatePlan(List<ProductionPlan3MModel> data);
        Task<ProductionPlanOverviewModel> Load(string id, int routeId);
        Task<ProductionPlanOverviewModel> Load3M(string planId);
        Task<ActiveProductionPlanModel> TakeAction(string id, int routeId);
        Task<ActiveMachine3MModel> TakeAction3M(string id, int machineId);
        FilterLoadProductionPlanListModel FilterLoadProductionPlan(int? productId, int? routeId, int? statusId, string planId, int? processTypeId);
    }
}
