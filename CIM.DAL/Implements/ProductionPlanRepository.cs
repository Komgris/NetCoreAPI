using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;

namespace CIM.DAL.Implements
{
    public class ProductionPlanRepository : Repository<ProductionPlan>, IProductionPlanRepository
    {
        public ProductionPlanRepository(cim_dbContext context) : base(context)
        {
        }

        public async Task<PagingModel<ProductionPlanListModel>> ListAsPaging(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive, string statusIds)
        {
            List<ProductionPlanListModel> data = null;
            //int totalCount = 0;
            var proc = _entities.LoadStoredProc("[sp_ListProductionPlan]");
            proc.AddParam("total_count", out IOutParam<int> totalCount);
            proc.AddParam("@route_id", routeId);
            proc.AddParam("@product_id", productId);
            proc.AddParam("@keyword", keyword);
            proc.AddParam("@is_active", isActive);
            proc.AddParam("@status_id", statusIds);
            proc.AddParam("@howmany", howmany);
            proc.AddParam("@page", page);
            await proc.ExecAsync(x => Task.Run(() => data = x.ToList<ProductionPlanListModel>()));

            return ToPagingModel(data, totalCount.Value, page, howmany);

        }

        //public async Task<int> ProductPlanOutputCount(int planId)
        //{
        //    int output;
        //    var proc = _entities.Lo
        //}
    }

}