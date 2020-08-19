using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.Domain.Models;
using CIM.Model;

namespace CIM.DAL.Interfaces
{
    public interface IProductionPlanRepository : IRepository<ProductionPlan, object>
    {
        Task<PagingModel<ProductionPlanListModel>> ListAsPaging(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive, string statusIds);
        FilterLoadProductionPlanListModel FilterLoadProductionPlan(int? productId, int? routeId, int? statusId,string planId);
        Task<ProductionPlanModel> Load(string id, int routeId);
        Task<List<ProductionPlanListModel>> ListByMonth(int month, int year, string statusIds);
        Task<List<ProductionOutputModel>> ListOutputByMonth(int month, int year);
        Task<PagingModel<ProductionOutputModel>> ListOutputByDate(DateTime date, int page, int howmany);
        Task<PagingModel<ProductionPlanListModel>> ListByDate(DateTime date, int page, int howmany, string statusIds);
        Task<PagingModel<ProductionOutputModel>> ListOutput(int page, int howmany, string keyword, bool isActive, string statusIds);
    }

}