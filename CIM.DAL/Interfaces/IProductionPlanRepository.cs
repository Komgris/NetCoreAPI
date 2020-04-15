using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.Domain.Models;
using CIM.Model;

namespace CIM.DAL.Interfaces
{
    public interface IProductionPlanRepository : IRepository<ProductionPlan>
    {
        Task<PagingModel<ProductionPlanListModel>> ListAsPaging(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive);
    }

}